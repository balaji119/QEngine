//*****************************************************************************
// DynTypeBuilder.cs - Helper class to create a dynamic type for the rows in
// database table.
//*****************************************************************************
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;

namespace QEngine
{
    class DynTypeBuilder
    {
        /// <summary>
        /// Creates new object for a given list of propery names
        /// </summary>
        /// <param name="props">Database row headder</param>
        /// <returns>New object</returns>
        public static object CreateNewObject(IEnumerable<string> props)
        {
            var type = CompileResultType(props);
            return Activator.CreateInstance(type);
        }

        /// <summary>
        /// Creates type by creating constuctor and properties.
        /// </summary>
        /// <param name="props">Collection of string to be used for creating properties</param>
        /// <returns>new Type</returns>
        public static Type CompileResultType(IEnumerable<string> props)
        {
            TypeBuilder tb = GetTypeBuilder();
            ConstructorBuilder constructor = tb.DefineDefaultConstructor(MethodAttributes.Public);
            foreach (var field in props)
                CreateProperty(tb, field, typeof(string));
            Type objectType = tb.CreateType();
            return objectType;
        }

        /// <summary>
        /// Creates a type builder from module builder and assembly builder
        /// </summary>
        /// <returns>TypeBuilder</returns>
        static TypeBuilder GetTypeBuilder()
        {
            var typeSignature = "DynamicType";
            var an = new AssemblyName(typeSignature);
            AssemblyBuilder assemblyBuilder = AppDomain.CurrentDomain.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType(typeSignature, TypeAttributes.Public | TypeAttributes.Class, null);
            return tb;
        }

        /// <summary>
        /// Creates property with given name and type.
        /// </summary>
        /// <param name="tb">TypeBuilder</param>
        /// <param name="propertyName">Name of the property</param>
        /// <param name="propertyType">Type of tee property to be create.</param>
        static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMethodBuilder = tb.DefineMethod("get_" + propertyName, MethodAttributes.Public, propertyType, Type.EmptyTypes);
            ILGenerator getterGen = getPropMethodBuilder.GetILGenerator();

            getterGen.Emit(OpCodes.Ldarg_0);
            getterGen.Emit(OpCodes.Ldfld, fieldBuilder);
            getterGen.Emit(OpCodes.Ret);

            MethodBuilder setPropMethodBuilder = tb.DefineMethod("set_" + propertyName, MethodAttributes.Public, null, new[] { propertyType });

            ILGenerator setterGen = setPropMethodBuilder.GetILGenerator();
            Label modifyProperty = setterGen.DefineLabel();
            Label exitSet = setterGen.DefineLabel();

            setterGen.MarkLabel(modifyProperty);
            setterGen.Emit(OpCodes.Ldarg_0);
            setterGen.Emit(OpCodes.Ldarg_1);
            setterGen.Emit(OpCodes.Stfld, fieldBuilder);

            setterGen.Emit(OpCodes.Nop);
            setterGen.MarkLabel(exitSet);
            setterGen.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMethodBuilder);
            propertyBuilder.SetSetMethod(setPropMethodBuilder);
        }
    }
}
