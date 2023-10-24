using System;
using CompAndDel.Pipes;
using CompAndDel.Filters;
using Ucu.Poo.Twitter;
using Ucu.Poo.Cognitive;

namespace CompAndDel
{
    class Program
    {
        static void Main(string[] args)
        {
           // Cargar una imagen en un IPicture
            PictureProvider provider = new PictureProvider();
            IPicture image = provider.GetPicture(@"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg");

            // Crear filtros
            IFilter blurFilter = new FilterBlurConvolution();
            IFilter greyscaleFilter = new FilterGreyscale();
            IFilter negativeFilter = new FilterNegative();

            IFilter twitterPublishFilter = new FilterGreyscale(); 

            // Crear pipes
            IPipe pipeNull = new PipeNull();
            IPipe pipeFork1 = new PipeFork(pipeNull, pipeNull);
            IPipe pipeFork2 = new PipeFork(pipeNull, pipeFork1);
            IPipe pipeSerial = new PipeSerial(negativeFilter, pipeFork2);
            IPipe pipeMain = new PipeSerial(greyscaleFilter, pipeSerial);

            // Ejecutar la secuencia de pipes y filtros
            IPicture filteredImage = (IPicture)pipeMain.Send(image);
            // Ejecutar la secuencia de pipes y filtros hasta el filtro "negativo"
            IPicture intermediateImage = (IPicture)pipeMain.Send(image);

            CognitiveFace cognitiveFace = new CognitiveFace();

            // Realizar el reconocimiento en una imagen
            cognitiveFace.Recognize(@"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_CognitiveAPI\src\Program\bill2.jpg");

            // Verificar si se encontró una cara
            bool faceFound = cognitiveFace.FaceFound;

            // Aplicar filtros en función del resultado
            if (faceFound)
            {
                // Aplicar el filtro que deseas si se encontró una cara
                IFilter filterIfFaceFound = new FilterBlurConvolution();
                filteredImage = filterIfFaceFound.Filter(image);
            }
            else
            {
                // Aplicar otro filtro si no se encontró una cara
                IFilter filterIfNoFaceFound = new FilterNegative(); 
                filteredImage = filterIfNoFaceFound.Filter(image);
            }

            // Publicar la imagen intermedia en Twitter
            var twitter = new TwitterImage();
            string tweetText = "prueba";
            string tweetResult = twitter.PublishToTwitter(tweetText, @"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg");

            Console.WriteLine(tweetResult);

            IFilter persistenceFilter = new FilterImagePersistence(@"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg", provider);
            intermediateImage = persistenceFilter.Filter(intermediateImage);
            provider.SavePicture(intermediateImage, @"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg");

            // Guardar la imagen resultante
            provider.SavePicture(filteredImage, @"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg");
            // Finalmente, guardar la imagen resultante
            provider.SavePicture(intermediateImage, @"C:\Users\felip\OneDrive - Universidad Católica del Uruguay\Facultad\Programacion II\repos\PII_Pipes_Filters_Start\src\Program\beer.jpg");
        }
    }
}
