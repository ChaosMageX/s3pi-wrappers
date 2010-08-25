using s3piwrappers.Collada.Interface;

namespace s3piwrappers.Collada.Common
{
    public class TokenArray : ColladaArray<string>
    {
        protected override string Parse(string s)
        {
            return s;
        }
    }
}