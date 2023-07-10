using DocumentFormat.OpenXml.Office2010.Excel;
using Entities.DbModels;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Services;
using System.Linq;

namespace api.Helpers
{

    public static class EnumPermisos
    {
        public const string GestionInformes = "GestionInformes";
        public const string TableroGestion = "TableroGestion";
        public const string TratamientoBrecha = "TratamientoBrecha";
        public const string Descargas = "Descargas";
    }
}
