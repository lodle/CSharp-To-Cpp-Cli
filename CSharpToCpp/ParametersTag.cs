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

                result.Write("{0} _{1}", p.NativeType, p.Name);
                first = false;
            }
        }
    }

    public class ParametersCallTag : DotLiquid.Tag
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

                result.Write(p.CallName);
                first = false;
            }
        }
    }
}
