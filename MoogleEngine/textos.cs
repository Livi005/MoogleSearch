namespace MoogleEngine
{
    public static class Normalizacion //clase de normalizacion de textos en general.
    {
    
        static string Normalizar(string textos) //quitar mayusculas y tildes de textos.
        {
            string texto1 = textos.ToLower();
            texto1 = texto1.Replace('á','a');
            texto1 = texto1.Replace('é','e');
            texto1 = texto1.Replace('í','i');
            texto1 = texto1.Replace('ó','o');
            texto1 = texto1.Replace('ú','u');
            return texto1;
        }
    
        public static string[] Separar(string texto1) // separar las palabras de textos.
        {
           char[] separador = {'.','?', ',', ';', '(', ')', '[', ']', '\n', '\r', ':', ' ', '*', '!', '^', '~', '\t', '\\'};
    
           return texto1.Split(separador, StringSplitOptions.RemoveEmptyEntries);  
        }
    
        public static List<string> Hazlo_todo(string texto) // normalizacion y separacion de textos*******.
        {
            string texto1 = Normalizar(texto);
            List<string> textos = Separar(texto1).ToList();
    
            return textos;
        }
    
    }
//Implementa de los metodos anteriores pero ya en los textos en especifico.
    class Procesamiento_txt // Procesamiento de los txt. 
    {
        public static (List<string>, List<string>) Leer() // Leer los txt y los titulos que tengo en la PC.
        {
            string[] directorio = Directory.GetFiles("../contents"); 
            string[] textos = new string[directorio.Length];
            string[] titulos = new string[directorio.Length];
            for(int i = 0; i < textos.Length; i++)
            {
                StreamReader leer = new StreamReader(directorio[i], System.Text.Encoding.UTF7);
                textos[i] = leer.ReadToEnd();// leer textos.
                titulos[i] = Path.GetFileName(directorio[i]);//leer titulos.
            }
            return (titulos.ToList(), textos.ToList());
        }

        //Convierto mi lista de listas en una listas de diccionarios donde cada diccionario es un texto con las palabras que tiene y la cantidad de veces que se repiten.
        public static List<Dictionary<string,int>> CreateDictionary(List<List<string>> textos)
        {
            List<Dictionary<string,int>> result = new List<Dictionary<string,int>>();
            for (int i = 0; i < textos.Count; i++)
            {    
                int val = 0;
                result.Add(new Dictionary<string, int>());
                for (int j = 0; j < textos[i].Count; j++)
                {
                   if(result[i].TryGetValue(textos[i][j],out val))
                    result[i][textos[i][j]] = val + 1;
                    else 
                    result[i].Add(textos[i][j],1);
                }
            }
            return result;
        }
        // Llamar a los metodos de normalizacion de los txt que estan en la clase anterior. 
        public static List<List<string>> Procesar(List<string> textos) 
        {

            List<List<string>> lista = new List<List<string>>();
    
            for(int i = 0; i < textos.Count; i++)
            {
                lista.Add(Normalizacion.Hazlo_todo(textos[i]));
            }
            
            return lista;
        }
    
        //Cantidad de palabras(25) que saldran despues de la busqueda, en los textos de resultado.(Snippet). el tf idf me da en que textos estan las palabras lo que me ayuda a saber la posicion.
        public static string Recorte_de_textos(float[] Idf, List<(string, float)> quary1, string texto)
        { 
            int i;

            string recorte = "";
            List<string> txt = Normalizacion.Hazlo_todo(texto);
            string[] textoSeparadoNOnormalizado = Normalizacion.Separar(texto);

            //me coge la pasicion de la palabra mas relevante de la query. si no esta me salta para la segunda mas relevante
            for (int j = 0; j < quary1.Count; j++)
            {
                if(txt.IndexOf(quary1[j].Item1) != -1) 
                {
                    int posicion = txt.IndexOf(quary1[j].Item1);

                    //cojo 33 empiezo a coger 33 palabras antes de la mas relevante la agrego 33 mas.
                    for (i = Math.Max(0, posicion - 33) ; i < Math.Min(posicion + 33, textoSeparadoNOnormalizado.Length); i++)
                    {
                    
                       recorte += textoSeparadoNOnormalizado[i] + " ";                     

                    }
                    break;
                }
            }

            return recorte;                
        }
    }
}


