using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace ProyectoFinalProgramacion
{
    public class Hasher
    {
        public static string HashearContrasena(string contrasena, out byte[] sal)
        {
            sal = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(sal);
            }

            var pbkdf2 = new Rfc2898DeriveBytes(contrasena, sal, 100_000, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(32); // genera un hash de 256 bits (32 bytes)

            return Convert.ToBase64String(hash);
        }
        public static bool VerificarContrasena(string contrasenaIngresada, byte[] salGuardada, string hashGuardado)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(contrasenaIngresada, salGuardada, 100_000, HashAlgorithmName.SHA256);
            byte[] hashIngresado = pbkdf2.GetBytes(32);
            string hashIngresadoBase64 = Convert.ToBase64String(hashIngresado);

            return hashIngresadoBase64 == hashGuardado;
        }
    }
}
