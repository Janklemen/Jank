using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using Jank.Utilities.Paths;
using Octokit;
using UnityEditor;
using UnityEngine;
using Application = UnityEngine.Application;
using CompressionLevel = System.IO.Compression.CompressionLevel;

namespace Jank.Editor
{
    public static class Release
    {
        [MenuItem("Janklemen/Release")]
        public static async void ReleaseStuff()
        {
            PathString projectBuildPath = new PathString(Application.dataPath)
                .RemoveAtEnd()
                .InsertAtEnd("Build");

            projectBuildPath.CreateIfNotExistsDirectory();
            DirectoryInfo buildDir = new(projectBuildPath);
            buildDir.Delete(true);
            buildDir.Create();

            string companyName = PlayerSettings.companyName;
            string productName = PlayerSettings.productName;
            string version = PlayerSettings.bundleVersion;

            string zipName = $"{companyName}-{productName}-{version}".Replace('.', '-') + ".zip";
            PathString zipPath = projectBuildPath.InsertAtEnd(zipName);

            PathString githubAuthPath = new PathString(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile))
                .InsertAtEnd(".github")
                .InsertAtEnd("auth");

            if (!githubAuthPath.TryAsFileInfo(out FileInfo githubAuthFile))
                throw new Exception($"Could not find file {githubAuthPath}");

            string githubAuthKey;
            string repoOwner = "hexthedev";
            string repoName = "RougeTacToe";

            await using (FileStream fs = githubAuthFile.OpenRead())
            {
                using (StreamReader sr = new(fs))
                    githubAuthKey = await sr.ReadToEndAsync();
            }

            // Do the builds
            BuildPlayerOptions options = new()
            {
                scenes = EditorBuildSettings.scenes.Select(set => set.path).ToArray(),
                locationPathName = projectBuildPath.InsertAtEnd($"{productName}.exe"),
                targetGroup = BuildTargetGroup.Standalone,
                target = BuildTarget.StandaloneWindows64
            };
            
            BuildPipeline.BuildPlayer(options);
            
            // Zip the build
            string zipActionTitle = $"Zipping Build title.zip";
            float totalThingsToCompress = buildDir.GetFiles().Length + buildDir.GetDirectories().Length;
            int totalThingsCompressed = 0;
            
            using (ZipArchive zip = ZipFile.Open(zipPath, ZipArchiveMode.Create))
            {
                foreach (FileInfo file in buildDir.GetFiles())
                {
                    if (file.FullName != zipPath.ToString('/') && file.FullName != zipPath.ToString('\\'))
                    {
                        zip.CreateEntryFromFile(file.FullName, file.Name, CompressionLevel.Optimal);
                        EditorUtility.DisplayProgressBar(zipActionTitle, $"Zipping file: {file.FullName}",
                            totalThingsCompressed / totalThingsToCompress);
                        totalThingsCompressed++;
                    }
                }
            
                foreach (DirectoryInfo dir in buildDir.GetDirectories())
                {
                    zip.CreateEntryFromDirectory(dir.FullName, dir.Name, CompressionLevel.Optimal);
                    EditorUtility.DisplayProgressBar(zipActionTitle, $"Zipping directory: {dir.FullName}",
                        totalThingsCompressed / totalThingsToCompress);
                    totalThingsCompressed++;
                }
            }
            
            EditorUtility.ClearProgressBar();

            // Find the latest commit
            string gitEditorBarTitle = "Releasing On Github";
            float totalGithubActions = 4;
            EditorUtility.DisplayProgressBar(gitEditorBarTitle, "Finding last commit", 0/totalGithubActions);

            var githubClient = new GitHubClient(new ProductHeaderValue("janklemen-release"));
            var tokenAuth = new Credentials(githubAuthKey); // This can be a PAT or an OAuth token.
            githubClient.Credentials = tokenAuth;

            var commits = await githubClient.Repository.Commit.GetAll(repoOwner, repoName, new ApiOptions()
            {
                PageCount = 1,
                PageSize = 1
            });
            var commit = commits.First();
            
            // Tag the main branch in github
            EditorUtility.DisplayProgressBar(gitEditorBarTitle, "Tagging commit", 1/totalGithubActions);
            NewTag tag = new()
            {
                Message = version,
                Object = commit.Sha,
                Tag = version,
                Tagger = new Committer(repoOwner, "jamesmccafferty@live.ca", DateTimeOffset.Now),
                Type = TaggedType.Commit
            };

            await githubClient.Git.Tag.Create(repoOwner, repoName, tag);
            
            // Do a release
            EditorUtility.DisplayProgressBar(gitEditorBarTitle, "Creating release", 2/totalGithubActions);
            var newRelease = new NewRelease(version)
            {
                Name = version,
                GenerateReleaseNotes = true,
                Prerelease = true
            };

            var release = await githubClient.Repository.Release.Create(repoOwner, repoName, newRelease);
            
            // Upload the zip
            EditorUtility.DisplayProgressBar(gitEditorBarTitle, $"Uploading {zipName}", 3/totalGithubActions);
            var assetUpload = new ReleaseAssetUpload() 
            {
                FileName = zipName,
                ContentType = "application/zip",
                RawData = File.OpenRead(zipPath)
            };
            await githubClient.Repository.Release.UploadAsset(release, assetUpload);
            
            EditorUtility.ClearProgressBar();
        }

        [MenuItem("Janklemen/Increment Project Version")]
        public static void IncrementSemanticVersion()
        {
            string version = PlayerSettings.bundleVersion;
            string[] dotSplit = version.Split('.');

            if (dotSplit.Length != 3)
                throw new Exception("Not a semantic version");

            if (dotSplit[2].Contains("-"))
            {
                string[] finalSplit = dotSplit[2].Split("-");
                finalSplit[0] = (int.Parse(finalSplit[0]) + 1).ToString();
                dotSplit[2] = String.Join("-", finalSplit);
            }
            else
                dotSplit[2] = (int.Parse(dotSplit[2]) + 1).ToString();

            PlayerSettings.bundleVersion = String.Join('.', dotSplit);
            Debug.Log($"Incremented player version {version} -> {PlayerSettings.bundleVersion}");
        }
        
        public static void CreateEntryFromAny(this ZipArchive archive, string sourceName, string entryName,
            CompressionLevel compressionLevel)
        {
            var fileName = Path.GetFileName(sourceName);
            if (File.GetAttributes(sourceName).HasFlag(FileAttributes.Directory))
            {
                archive.CreateEntryFromDirectory(sourceName, Path.Combine(entryName, fileName), compressionLevel);
            }
            else
            {
                archive.CreateEntryFromFile(sourceName, Path.Combine(entryName, fileName), compressionLevel);
            }
        }

        public static void CreateEntryFromDirectory(this ZipArchive archive, string sourceDirName, string entryName,
            CompressionLevel compressionLevel)
        {
            string[] files = Directory.GetFiles(sourceDirName).Concat(Directory.GetDirectories(sourceDirName))
                .ToArray();
            archive.CreateEntry(Path.Combine(entryName, Path.GetFileName(sourceDirName)));
            foreach (var file in files)
            {
                archive.CreateEntryFromAny(file, entryName, compressionLevel);
            }
        }
    }
}