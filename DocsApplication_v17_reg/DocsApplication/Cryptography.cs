﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace DocsApplication
{
    class Cryptography
    {
        public enum CryptMethod { ENCRYPT, DECRYPT }
        public enum CryptClass { AES, RC2, RIJ, DES, TDES }
        public class Generic
        {

            public object Crypt(CryptMethod _method, CryptClass _class, object _input, string _key)
            {
                SymmetricAlgorithm control;
                switch (_class)
                {
                    case CryptClass.AES:
                        control = new AesManaged();
                        break;
                    default:
                        return false;
                        break;
                }

                control.Key = ASCIIEncoding.ASCII.GetBytes(_key);
                control.Padding = PaddingMode.PKCS7;
                control.Mode = CipherMode.ECB;

                ICryptoTransform cTransform = null;
                byte[] resultArray;

                if (_method == CryptMethod.ENCRYPT)
                {
                    cTransform = control.CreateEncryptor();
                }
                else if (_method == CryptMethod.DECRYPT)
                {
                    cTransform = control.CreateDecryptor();
                }
                if (_input is byte[])
                {
                    resultArray = cTransform.TransformFinalBlock((_input as byte[]), 0, (_input as byte[]).Length);
                    control.Clear();
                    return resultArray;

                }
                return false;
            }
        }
    }
}
