using MagicVilla.Models.Dto;

namespace MagicVilla.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList =  new List<VillaDTO> {
                new VillaDTO { Id = 1, Name = "Beach View", Occupancy=1000, Sqft=3000 },
                new VillaDTO { Id = 2, Name = "Pool View", Occupancy=2000, Sqft=5000 }
        };

    }
}
