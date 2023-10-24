using System;
using CompAndDel;

namespace CompAndDel.Filters
{
    public class FilterImagePersistence : IFilter
    {
        private string filePath;
        private PictureProvider pictureProvider;

        public FilterImagePersistence(string filePath, PictureProvider pictureProvider)
        {
            this.filePath = filePath;
            this.pictureProvider = pictureProvider;
        }

        public IPicture Filter(IPicture image)
        {
            // Guardar la imagen en el archivo especificado
            pictureProvider.SavePicture(image, filePath);

            // Devolver la imagen original sin cambios
            return image;
        }
    }
}
