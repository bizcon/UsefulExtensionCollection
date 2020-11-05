using System;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Security.Cryptography.X509Certificates;

namespace Bizcon.Extensions
{
    public static class CertificateExtensions
    {
#nullable enable
        public static X509Certificate2 FromStore(this X509Certificate2 certificate, string store, string findBy, object findValue)
        {
            StoreLocation storeLocation = (StoreLocation)Enum.Parse(typeof(StoreLocation), store);
            X509FindType findType = (X509FindType)Enum.Parse(typeof(X509FindType), findBy);
            if (string.IsNullOrEmpty(store) || string.IsNullOrEmpty(findBy) || null == findValue)
            {
                throw new ArgumentException("missing/wrong configuration/parameters");
            }

            return certificate.FromStore(storeLocation, findType, findValue);

        }
#nullable restore

#nullable enable
        public static X509Certificate2 FromStore(this X509Certificate2 certificate, StoreLocation store, X509FindType findBy, object findValue)
        {
            using (var certificateStore = new X509Store(store))
            {
                certificateStore.Open(OpenFlags.OpenExistingOnly | OpenFlags.ReadOnly);
                var certs = certificateStore.Certificates
                    .Find(findBy, findValue, false);
                if (null != certs && 1 == certs.Count)
                {
                    certificate = certs.OfType<X509Certificate2>().Single();
                }
                else
                {
                    throw new ArgumentException("no matching certificate found with given configuration/parameters");
                }
                certificateStore.Close();

                return certificate;
            }
        }
#nullable restore
    }
}
