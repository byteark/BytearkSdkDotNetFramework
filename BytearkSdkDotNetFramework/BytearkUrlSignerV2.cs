using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BytearkSdkDotNetFramework
{
    /**
     * URL Signer for ByteArk CDN and Video Streaming Services.
     */
    public class BytearkUrlSignerV2
    {
        private static string schemeDelimeter = "://";

        private static string lineFeed = "\n";

        private string accessId;

        private string accessSecret;

        private SignOptions defaultSignOptions;

        public BytearkUrlSignerV2(string accessId, string accessSecret)
        {
            this.accessId = accessId;
            this.accessSecret = accessSecret;
            this.defaultSignOptions = new SignOptions();
        }
        public BytearkUrlSignerV2(string accessId, string accessSecret, SignOptions defaultSignOptions)
        {
            this.accessId = accessId;
            this.accessSecret = accessSecret;
            this.defaultSignOptions = defaultSignOptions;
        }

        public string Sign(string url, int expiresUnixTimestamp)
        {
            return Sign(new Uri(url), expiresUnixTimestamp, defaultSignOptions);
        }

        public string Sign(string url, int expiresUnixTimestamp, SignOptions options)
        {
            return Sign(new Uri(url), expiresUnixTimestamp, options);
        }

        public string Sign(Uri uri, int expiresUnixTimestamp)
        {
            return Sign(uri, expiresUnixTimestamp, defaultSignOptions);
        }

        public string Sign(Uri uri, int expiresUnixTimestamp, SignOptions options)
        {
            var sharedStringBuilder = new StringBuilder();

            var stringToSign = makeStringToSign(
                uri,
                expiresUnixTimestamp,
                options,
                sharedStringBuilder
            );
            var signature = makeSignature(stringToSign);
            var signedUrl = makeSignedUrl(
                uri,
                expiresUnixTimestamp,
                options,
                signature,
                sharedStringBuilder
            );

            return signedUrl;
        }

        protected string makeStringToSign(
            Uri uri,
            int expiresUnixTimestamp,
            SignOptions options,
            StringBuilder sharedStringBuilder
        )
        {
            var method = options.Method();

            sharedStringBuilder.Clear();
            sharedStringBuilder.Append(method);
            sharedStringBuilder.Append(lineFeed);
            sharedStringBuilder.Append(uri.Host);
            sharedStringBuilder.Append(lineFeed);
            if (options.HasPathPrefix())
            {
                sharedStringBuilder.Append(options.PathPrefix());
            }
            else
            {
                sharedStringBuilder.Append(uri.AbsolutePath);
            }
            sharedStringBuilder.Append(lineFeed);
            sharedStringBuilder.Append(expiresUnixTimestamp.ToString());
            sharedStringBuilder.Append(lineFeed);
            sharedStringBuilder.Append(accessSecret);

            var result = sharedStringBuilder.ToString();
            return result;
        }

        protected string makeSignature(string stringToSign)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(stringToSign);
                byte[] hashBytes = md5.ComputeHash(inputBytes);
                return Convert.ToBase64String(hashBytes)
                    .TrimEnd('=')
                    .Replace('+', '-')
                    .Replace('/', '_');
            }
        }

        protected string makeSignedUrl(
            Uri uri,
            int expiresUnixTimestamp,
            SignOptions options,
            string signature,
            StringBuilder sharedStringBuilder
        )
        {
            var firstQueryDelimeter = uri.Query.Length > 0 ? "&" : "?";

            sharedStringBuilder.Clear();
            sharedStringBuilder.Append(uri.Scheme);
            sharedStringBuilder.Append(schemeDelimeter);
            sharedStringBuilder.Append(uri.Host);
            sharedStringBuilder.Append(uri.AbsolutePath);
            sharedStringBuilder.Append(uri.Query);
            sharedStringBuilder.Append(firstQueryDelimeter);
            sharedStringBuilder.Append("x_ark_access_id=");
            sharedStringBuilder.Append(accessId);
            sharedStringBuilder.Append("&x_ark_auth_type=ark-v2");
            sharedStringBuilder.Append("&x_ark_expires=");
            sharedStringBuilder.Append(expiresUnixTimestamp.ToString());
            if (options.HasPathPrefix())
            {
                sharedStringBuilder.Append("&x_ark_path_prefix=");
                sharedStringBuilder.Append(Uri.EscapeDataString(options.PathPrefix()));
            }
            sharedStringBuilder.Append("&x_ark_signature=");
            sharedStringBuilder.Append(signature);

            return sharedStringBuilder.ToString();
        }

        /**
         * Options for making signed URLs.
         */
        public class SignOptions
        {
            private static string defaultMethod = "GET";

            private string method;
            private string pathPrefix;

            public SignOptions()
            {
                method = defaultMethod;
            }
            public string Method()
            {
                return method;
            }
            public SignOptions WithMethod(string method)
            {
                this.method = method;
                return this;
            }
            public string PathPrefix()
            {
                return pathPrefix;
            }
            public bool HasPathPrefix()
            {
                return pathPrefix != null;
            }
            public SignOptions WithPathPrefix(string pathPrefix)
            {
                this.pathPrefix = pathPrefix;
                return this;
            }
        }
        public static SignOptions NewSignOptions()
        {
            return new SignOptions();
        }
    }
}
