using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EncryptLibrary
{
    public class EncryptorConfiguration
    {
        public const string Name = "EncryptorConfiguration";
        public string PasswordIterations { get; set; }
        public string Salt {  get; set; }
        public string InitVector { get; set; }
        public string Passphrase { get; set; }
    }
}
