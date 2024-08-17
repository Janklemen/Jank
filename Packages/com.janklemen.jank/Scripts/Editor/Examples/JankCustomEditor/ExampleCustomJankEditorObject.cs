using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Jank.Inspector.CustomEditorGenerator;
using UnityEngine;

namespace Jank.Editor.Examples
{
    public class TestObject
    {
        public TestObject(object obj)
        {
            PublicObject = obj;
        }
        
        [JankSpace]
        [JankHeader("Public")]
        public int INTPublic = 5;
        public float FloatPublic = 5.0f;
        public double DoublePublic = 5.0d;
        public long LongPublic = 1500000000L;
        public short ShortPublic = 30;
        public byte BytePublic = 200;
        public bool BoolPublic = true;
        public char CharPublic = 'c';
        public string StringPublic = "non-default string";
        public Vector3 Vector3Public = new Vector3(1,2,3);
        public Vector2 Vector2Public = new Vector2(1,2);
        public Vector4 Vector4Public = new Vector4(1,2,3,4);
        public Quaternion QuaternionPublic = new Quaternion(1,2,3,4);
        public Color ColorPublic = Color.red;
        public Rect RectPublic = new Rect(1,1,2,2);
        public TestEnum EnumPublic = TestEnum.Vale3;
        public Transform UObjectPublic;
        
        [JankSpace]
        [JankHeader("Object")]
        public object PublicObject;
    }
    
    [CreateAssetMenu(fileName = "ExampleCustomJankEditorObject",
        menuName = "Jank/Examples/JankCustomEditor/ExampleCustomJankEditorObject")]
    [JankCustomEditor]
    public partial class ExampleCustomJankEditorObject : ScriptableObject
    {
        // Public basic types
        [JankSpace]
        [JankHeader("Public")]
        public int IntPublic = 5;
        public float FloatPublic = 5.0f;
        public double DoublePublic = 5.0d;
        public long LongPublic = 1500000000L;
        public short ShortPublic = 30;
        public byte BytePublic = 200;
        public bool BoolPublic = true;
        public char CharPublic = 'c';
        public string StringPublic = "non-default string";
        public Vector3 Vector3Public = new Vector3(1,2,3);
        public Vector2 Vector2Public = new Vector2(1,2);
        public Vector4 Vector4Public = new Vector4(1,2,3,4);
        public Quaternion QuaternionPublic = new Quaternion(1,2,3,4);
        public Color ColorPublic = Color.red;
        public Rect RectPublic = new Rect(1,1,2,2);
        public TestEnum EnumPublic = TestEnum.Vale2;
        public Transform UObjectPublic;
        
        // Public object tests
        [JankSpace]
        [JankHeader("Public Objects")]
        [JankInspect] public object PublicNullObject;
        [JankInspect] public object PublicTestObject = new TestObject(new TestObject(null));

        [JankSpace]
        [JankHeader("Private")]
#pragma warning disable CS0414 // Field is assigned but its value is never used
        [JankInspect] int _intPrivate = 10;
        [JankInspect] float _floatPrivate = 10.0f;
        [JankInspect] double _doublePrivate = 10.0d;
        [JankInspect] long _longPrivate = 3000000000L;
        [JankInspect] short _shortPrivate = 40;
        [JankInspect] byte _bytePrivate = 255;
        [JankInspect] bool _boolPrivate = true;
        [JankInspect] char _charPrivate = 'd';
        [JankInspect] string _stringPrivate = "non-default private string";
        [JankInspect] Vector3 _vector3Private = new Vector3(4,5,6);
        [JankInspect] Vector2 _vector2Private = new Vector2(3,4);
        [JankInspect] Vector4 _vector4Private = new Vector4(5,6,7,8);
        [JankInspect] Quaternion _quaternionPrivate = new Quaternion(5,6,7,8);
        [JankInspect] Color _colorPrivate = Color.blue;
        [JankInspect] Rect _rectPrivate = new Rect(3,3,4,4);
        [JankInspect] TestEnum _enumPrivate = TestEnum.Vale4;
        [JankInspect] Transform _uobjectPrivate;
#pragma warning restore CS0414 // Field is assigned but its value is never used

        [JankSpace]
        [JankHeader("Interfaces")]
        [JankInspect] public IExample InterfaceRef;
        [JankInspect] public List<IExample> InterfaceListRef;
        
        [JankSpace]
        [JankHeader("Functions")]
        [JankInspect]
        public UniTask Example(TestObjectConstructors test)
        {
            Debug.Log("Example");
            return UniTask.CompletedTask;
        }
    }


    public enum TestEnum
    {
        Vale1,
        Vale2,
        Vale3,
        Vale4,
        Vale5,
    }
    
    public class TestObjectConstructors
    {
        public TestObjectConstructors(int x, string y)
        {
            
        }
        
        public TestObjectConstructors(object z)
        {
            
        }
    }
}