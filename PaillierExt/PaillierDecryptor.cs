/************************************************************************************
 This is an implementation of the Paillier encryption scheme with support for
 homomorphic addition.

 This library is provided as-is and is covered by the MIT License [1].

 [1] The MIT License (MIT), website, (http://opensource.org/licenses/MIT)
 ************************************************************************************/

using System.Numerics;

namespace PaillierExt
{
    public class PaillierDecryptor : PaillierAbstractCipher
    {
        private static readonly BigInteger max = BigInteger.Pow(2, PaillierConfig.size) - BigInteger.One;
        private static readonly BigInteger exponent = BigInteger.Pow(10, PaillierConfig.exponent);    //Exponent of 100 to work with 2 decimal places

        public PaillierDecryptor(PaillierKeyStruct p_struct)
            : base(p_struct)
        {
            o_block_size = o_ciphertext_blocksize;
        }

        //TODO: check again for decryption
        public BigFraction ProcessByteBlock(byte[] p_block)
        {
            var block = new BigInteger(p_block);

            // calculate M
            // m = (c^lambda(mod nsquare) - 1) / n * miu (mod n)
            var m = (BigInteger.ModPow(block, o_key_struct.Lambda, o_key_struct.NSquare) - 1) / o_key_struct.N * o_key_struct.Miu % o_key_struct.N;

            return Decode(m);
        }

        private BigFraction Decode(BigInteger n)
        {
            var a = new BigFraction(n, exponent);
            a = a % (max + 1);
            if ( a > max / 2)
                a = a - max - 1;
            return a;
        }
    }
}
