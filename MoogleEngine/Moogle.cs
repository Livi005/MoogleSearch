namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) 
    {
        
        List<string> quary = Normalizacion.Hazlo_todo(query); //normalizacion d la query
        List<(string, float)> quary1 = Mi_Query.Repeticion_de_palabras(quary);// las palabras d la query con la cant d veces que se repiten

        (List<string>, List<string>) txt = Procesamiento_txt.Leer();
        List<List<string>> txt1 = Procesamiento_txt.Procesar(txt.Item2);//procesamiento de los textos

        List<Dictionary<string,int>> txt2 = Procesamiento_txt.CreateDictionary(txt1);//procesamiento de los textos

        List<string> semejantes = Sugerencias.Palabras_semejantes(quary1, txt2); //palabras semejantes (levenstain)
    

        List<string> exc;                    //operadores
        List<string> pico;                  //operadores
        List<(string, int)> aster ;         //operadores
        List<(string, string)> ene;        //operadores

        (exc,pico,aster,ene) = Mi_Query.Operadores(query); //palabras q estan alrededor de los operadores.
        
        bool[] b = Operadores.Mascara(txt2,exc,pico);

        float[] f;
        float[] idf;
        (f, idf)= Tf_Idf.Resultado(txt2, quary1,aster, ene, b, txt1);  //calculo del score segun los operadores d la query

        Tf_Idf.Ordenar(txt.Item1, txt.Item2, f);  //ordenar el resultado segun la cantidad del score.

        string suggestion = Sugerencias.Sugerir(quary1, idf, semejantes, txt1.Count-1);

        Tf_Idf.OrdenarPalabrasQuery(quary1, idf);
        
        SearchItem[] items = Moogle.Resultado(txt, quary1, f, idf);
        return new SearchResult(items, suggestion);
    }

    //el resultado de la busqueda (titulo, texto, valor del Tf_IDF).
    public static SearchItem[] Resultado((List<string>, List<string>) txt, List<(string, float)> quary1, float[] Tf_IDF, float[] IDF)
    {
        SearchItem busqueda;
        List<SearchItem> resultado = new List<SearchItem>(); // textos para poner en el resultado.

        for(int i = 0; i < 10 && i < txt.Item1.Count; i++)
        {
            if(Tf_IDF[i] <= 0) break;

            busqueda = new SearchItem(txt.Item1[i],Procesamiento_txt.Recorte_de_textos(IDF, quary1, txt.Item2[i]),Tf_IDF[i]);   
            resultado.Add(busqueda);
        }
        return resultado.ToArray();

    }
}
