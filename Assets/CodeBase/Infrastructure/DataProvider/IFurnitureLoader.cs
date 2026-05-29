using CodeBase.Data.FurnitureConstructor;

namespace CodeBase.Infrastructure.DataProvider
{
    public interface IFurnitureLoader
    {
        FurnitureData GetFurnitureData(string name);
        Database LoadDatabase();
    }
}
