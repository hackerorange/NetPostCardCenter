namespace soho.security
{
    public static class Security
    {
        private static string  _tokenId="admin";
        public static string TokenId
        {
            get { return  _tokenId; }
            set { _tokenId = value; }
        }
    }
}