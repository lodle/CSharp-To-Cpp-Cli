using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DotLiquid;
using System.IO;
using CSharpToCpp.Gen;

namespace CSharpToCpp
{
    public class ParametersTag : DotLiquid.Tag
    {
        List<String> _Parts;

        bool IsNativeToManaged;
        bool IsNativeInternal;
        bool IsNativeExternal;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var args = markup.Split(' ');
            _Parts = args[0].Split('.').ToList();

            IsNativeToManaged = (args.Length > 1 && args[1] == "NativeToManaged");
            IsNativeInternal = (args.Length > 1 && args[1] == "NativeInternal");
            IsNativeExternal = (args.Length > 1 && args[1] == "NativeExternal");

            for (int x = 0; x < _Parts.Count(); x++)
                _Parts[x] = _Parts[x].Trim();

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            DotLiquid.Hash hash = context[_Parts[0]] as DotLiquid.Hash;

            if (hash == null)
                return;

            List<GenParameter> list = hash[_Parts[1]] as List<GenParameter>;

            if (list == null)
                return;

            bool first = true;
            foreach (var p in list)
            {
                if (!first)
                    result.Write(", ");

                if (IsNativeToManaged || IsNativeInternal)
                {
                    if (p.Type == typeof(String))
                        result.Write("const char* _sz{0}Buff, size_t _n{0}Size", p.Name);
                    else
                        result.Write("{0} _{1}", p.NativeType, p.Name);
                }
                else if (IsNativeExternal)
                {
                    if (p.Type.IsClass && p.Type != typeof(String))
                        result.Write("C{0} &_{1}", p.NativeType.Replace("I*", ""), p.Name);
                    else
                        result.Write("{0} _{1}", p.NativeType, p.Name);
                }
                else
                {
                    result.Write("{0} _{1}", p.NativeType, p.Name);
                }

                first = false;
            }
        }
    }

    public class ParametersManagedTag : DotLiquid.Tag
    {
        List<String> _Parts;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            _Parts = markup.Split('.').ToList();

            for (int x = 0; x < _Parts.Count(); x++)
                _Parts[x] = _Parts[x].Trim();

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            DotLiquid.Hash hash = context[_Parts[0]] as DotLiquid.Hash;

            if (hash == null)
                return;

            List<GenParameter> list = hash[_Parts[1]] as List<GenParameter>;

            if (list == null)
                return;

            bool first = true;
            foreach (var p in list)
            {
                if (!first)
                    result.Write(", ");

                result.Write("{0} _{1}", p.ManagedType, p.Name);
                first = false;
            }
        }
    }

    public class ParametersCallTag : DotLiquid.Tag
    {
        List<String> _Parts;
        bool IsNativeToNative;
        bool IsNativeToManaged;

        bool IsManagedToNative;
        bool IsManagedToNativeSetup;

        bool IsInternalToExternal;
        bool IsInternalToExternalSetup;

        public override void Initialize(string tagName, string markup, List<string> tokens)
        {
            var args = markup.Split(' ');
            _Parts = args[0].Split('.').ToList();

            IsNativeToNative = (args.Length > 1 && args[1] == "NativeToNative");
            IsNativeToManaged = (args.Length > 1 && args[1] == "NativeToManaged");

            IsManagedToNative = (args.Length > 1 && args[1] == "ManagedToNative");
            IsManagedToNativeSetup = (args.Length > 1 && args[1] == "ManagedToNativeSetup");

            IsInternalToExternal = (args.Length > 1 && args[1] == "InternalToExternal");
            IsInternalToExternalSetup = (args.Length > 1 && args[1] == "InternalToExternalSetup");
            
            for (int x = 0; x < _Parts.Count(); x++)
                _Parts[x] = _Parts[x].Trim();

            base.Initialize(tagName, markup, tokens);
        }

        public override void Render(Context context, TextWriter result)
        {
            DotLiquid.Hash hash = context[_Parts[0]] as DotLiquid.Hash;

            if (hash == null)
                return;

            List<GenParameter> list = hash[_Parts[1]] as List<GenParameter>;

            if (list == null)
                return;

            bool first = true;
            foreach (var p in list)
            {
                if (!first)
                    result.Write(", ");

                first = false;

                if (IsNativeToNative)
                {
                    if (p.Type == typeof(String))
                        result.Write("_{0}.c_str(), _{0}.size()", p.Name);
                    else
                        result.Write("_{0}", p.Name);
                }
                else if (IsInternalToExternalSetup)
                {
                    first = true;

                    if (p.Type.IsClass && p.Type != typeof(String))
                    {
                        result.WriteLine("C{0} ref{1}(_{1});", p.NativeType.Replace("I*", ""), p.Name);
                    }
                }
                else if (IsInternalToExternal)
                {
                    if (p.Type == typeof(String))
                        result.Write("std::string(_sz{0}Buff, _n{0}Size)", p.Name);
                    else if (p.Type.IsClass)
                        result.Write("ref{0}", p.Name);
                    else
                        result.Write("_{0}", p.Name);
                }
                else if (IsManagedToNativeSetup)
                {
                    if (p.Type == typeof(String))
                        result.WriteLine("std::string sz{0} = {1};", p.Name, p.CallName);

                    first = true;
                }
                else if (IsManagedToNative)
                {
                    if (p.Type == typeof(String))
                        result.Write("sz{0}.c_str(), sz{0}.size()", p.Name);
                    else
                        result.Write(p.CallName);
                }
                else
                {
                    result.Write(p.CallName);
                }
            }
        }
    }
}
