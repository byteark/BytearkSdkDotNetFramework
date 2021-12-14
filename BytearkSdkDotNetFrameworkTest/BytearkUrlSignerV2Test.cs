using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using BytearkSdkDotNetFramework;

namespace BytearkSdkDotNetFrameworkTest
{
    [TestClass]
    public class BytearkUrlSignerV2Test
    {
        [TestMethod]
        public void TestSignerReturnsValidSignedUrl()
        {
            var signer = new BytearkUrlSignerV2(
                "2Aj6Wkge4hi1ZYLp0DBG",
                "31sX5C0lcBiWuGPTzRszYvjxzzI3aCZjJi85ZyB7"
            );
            var signedUrl = signer.Sign(
              "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8",
              1514764800
            );
            var expectedSignedUrl = "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8"
                + "?x_ark_access_id=2Aj6Wkge4hi1ZYLp0DBG"
                + "&x_ark_auth_type=ark-v2"
                + "&x_ark_expires=1514764800"
                + "&x_ark_signature=cLwtn96a-YPY7jt8ZKSf_Q";
            Assert.AreEqual(expectedSignedUrl, signedUrl);
        }

        [TestMethod]
        public void TestSignerReturnsValidSignedUrlEvenIfItIsCalledTwice()
        {
            var signer = new BytearkUrlSignerV2(
                "2Aj6Wkge4hi1ZYLp0DBG",
                "31sX5C0lcBiWuGPTzRszYvjxzzI3aCZjJi85ZyB7"
            );
            var signedUrl = signer.Sign(
              "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8",
              1514764800
            );
            var expectedSignedUrl = "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8"
                + "?x_ark_access_id=2Aj6Wkge4hi1ZYLp0DBG"
                + "&x_ark_auth_type=ark-v2"
                + "&x_ark_expires=1514764800"
                + "&x_ark_signature=cLwtn96a-YPY7jt8ZKSf_Q";
            Assert.AreEqual(expectedSignedUrl, signedUrl);

            var anotherSignedUrl = signer.Sign(
              "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8",
              1514764800
            );
            var anotherEpectedSignedUrl = "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8"
                + "?x_ark_access_id=2Aj6Wkge4hi1ZYLp0DBG"
                + "&x_ark_auth_type=ark-v2"
                + "&x_ark_expires=1514764800"
                + "&x_ark_signature=cLwtn96a-YPY7jt8ZKSf_Q";
            Assert.AreEqual(anotherSignedUrl, anotherEpectedSignedUrl);
        }

        [TestMethod]
        public void TestSignerReturnsValidSignedUrlWithCustomHttpMethod()
        {
            var signer = new BytearkUrlSignerV2(
                "2Aj6Wkge4hi1ZYLp0DBG",
                "31sX5C0lcBiWuGPTzRszYvjxzzI3aCZjJi85ZyB7"
            );
            var signedUrl = signer.Sign(
                "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8",
                1514764800,
                BytearkUrlSignerV2
                    .NewSignOptions()
                    .WithMethod("HEAD")
            );
            var expectedSignedUrl = "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8"
                + "?x_ark_access_id=2Aj6Wkge4hi1ZYLp0DBG"
                + "&x_ark_auth_type=ark-v2"
                + "&x_ark_expires=1514764800"
                + "&x_ark_signature=QULE8DQ08f8fhFC-1gDUWQ";
            Assert.AreEqual(expectedSignedUrl, signedUrl);
        }

        [TestMethod]
        public void TestSignerReturnsValidSignedUrlWithPathPrefix()
        {
            var signer = new BytearkUrlSignerV2(
                "2Aj6Wkge4hi1ZYLp0DBG",
                "31sX5C0lcBiWuGPTzRszYvjxzzI3aCZjJi85ZyB7"
            );
            var signedUrl = signer.Sign(
                "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8",
                1514764800,
                BytearkUrlSignerV2
                    .NewSignOptions()
                    .WithPathPrefix("/video-objects/QDuxJm02TYqJ/")
            );
            var expectedSignedUrl = "http://inox.qoder.byteark.com/video-objects/QDuxJm02TYqJ/playlist.m3u8"
                + "?x_ark_access_id=2Aj6Wkge4hi1ZYLp0DBG"
                + "&x_ark_auth_type=ark-v2"
                + "&x_ark_expires=1514764800"
                + "&x_ark_path_prefix=%2Fvideo-objects%2FQDuxJm02TYqJ%2F"
                + "&x_ark_signature=334wInm0jKfC6LCm23zndA";
            Assert.AreEqual(expectedSignedUrl, signedUrl);
        }
    }
}
