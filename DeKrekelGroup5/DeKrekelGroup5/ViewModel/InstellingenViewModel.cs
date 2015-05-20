using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DeKrekelGroup5.Models.Domain;

namespace DeKrekelGroup5.ViewModel
{
    public class InstellingenViewModel:IValidatableObject
    {
        [Required(ErrorMessage = "Vul de  maximum toegestane dagen van uitlening in aub...")]
        [Range(1, 99, ErrorMessage = "Vul een getal tussen 1 en 99  in aub...")]
        public int UitleenDagen { get; set; }
        [Required(ErrorMessage = "Vul een maximum aantal toegestane verlengingen in tussen 0 & 10 in aub...")]
        [Range(0, 10, ErrorMessage = "Vul een maximum aantal toegestane verlengingen in tussen 0 & 10 in aub...")]
        public int MaxVerlengingen { get; set; }

        
        [MaxLength(12, ErrorMessage = "Tussen 6 & 12 tekens")]
        [MinLength(6, ErrorMessage = "Tussen 6 & 12 tekens")]
        public string BeheerderPaswoord { get; set; }
        [MaxLength(12, ErrorMessage = "Tussen 6 & 12 tekens")]
        [MinLength(6, ErrorMessage = "Tussen 6 & 12 tekens")]
        public string BeheerderPaswoordBevestigd { get; set; }

        [MaxLength(12, ErrorMessage = "Tussen 6 & 12 tekens")]
        [MinLength(6, ErrorMessage = "Tussen 6 & 12 tekens")]
        public string BibliothecarisPaswoord { get; set; }
        [MaxLength(12, ErrorMessage = "Tussen 6 & 12 tekens")]
        [MinLength(6, ErrorMessage = "Tussen 6 & 12 tekens")]
        public string BibliothecarisPaswoordBevestigd { get; set; }


        public InstellingenViewModel()
        {

        }
        public InstellingenViewModel(Instellingen instellingen)
        {
            UitleenDagen = instellingen.UitleenDagen;
            MaxVerlengingen = instellingen.MaxVerlengingen;
            BeheerderPaswoord = "";
            BeheerderPaswoordBevestigd = "";
            BibliothecarisPaswoord = "";
            BibliothecarisPaswoordBevestigd = "";
        }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if ((!string.IsNullOrEmpty(BeheerderPaswoord) && string.IsNullOrEmpty(BeheerderPaswoordBevestigd)) || (string.IsNullOrEmpty(BeheerderPaswoord) && !string.IsNullOrEmpty(BeheerderPaswoordBevestigd)))
                yield return new ValidationResult("Paswoord en bevestig paswoord moeten beiden ingevuld of leeg zijn", new[] { "BeheerderPaswoord", "BeheerderPaswoordBevestigd" });

            if ((!string.IsNullOrEmpty(BibliothecarisPaswoord) && string.IsNullOrEmpty(BibliothecarisPaswoordBevestigd)) || (string.IsNullOrEmpty(BibliothecarisPaswoord) && !string.IsNullOrEmpty(BibliothecarisPaswoordBevestigd)))
                yield return new ValidationResult("Paswoord en bevestig paswoord moeten beiden ingevuld of leeg zijn", new[] { "BibliothecarisPaswoord", "BibliothecarisPaswoordBevestigd" });

            if (BeheerderPaswoord != BeheerderPaswoordBevestigd)
                yield return new ValidationResult("Paswoorden komen niet overeen", new[] { "BeheerderPaswoord", "BeheerderPaswoordBevestigd" });

            if (BibliothecarisPaswoord != BibliothecarisPaswoordBevestigd)
                yield return new ValidationResult("Paswoorden komen niet overeen", new[] { "BibliothecarisPaswoord", "BibliothecarisPaswoordBevestigd" });

        }
    }
}