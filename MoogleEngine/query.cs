namespace MoogleEngine
{
    public class Mi_Query 
    {
        // lista con las palabras de la query y la cantidad de veces que se repiten en la query.
        public static List<(string, float)> Repeticion_de_palabras(List<string> query)
        {
            // mascara booleana para saber cuando esta repetida una palabra.
            bool[] b = new bool[query.Count];
            //Lista de las palabras con la cantidad de las veces que se repiten en la query.
            List<(string, float)> Repeticion = new List<(string, float)>();
    
            for (int i = 0; i < query.Count; i++)
            {
                if(!b[i]) // (if) que me permite no coger mas de una vez la misma palabra.
                {
                    int contador = 1; 
                    b[i] = true; // me pone en true las palabras que voy encontrando en el recorrido por la query.
                    for(int j = i + 1; j < query.Count; j++)
                    {
                        if(query[i] == query[j]) 
                        {
                            b[j] = true;
                            contador++;   //sumo uno a la repeticion.
                        }
                    } 
    
                    Repeticion.Add((query[i], contador));
                }
            }
    
            return Repeticion;
        }  

        //me da la palabra que esta detras del operador (depende del operador q tiene) 
        static string Dame_una_palabra(int i, string query)
        {
            string palabra = "";
            for (int j = i + 1; j < query.Length && query[j] != ' ' ; j++)
            {
                palabra += query[j];
            }
            return palabra;
        }
        //me da la palabra delante del operador (~). 
        static string Dame_una_palabra2(int i, string query)
        {
            string palabra2 = "";
            for (int j = i - 1; j > 0 && query[j] != ' '; j--)
            {
                palabra2 = query[j]+palabra2;
            }
            return palabra2;
        }

        
        public static (List<string>, List<string>, List<(string, int)>, List<(string, string)>) Operadores(string query)
        {
            // listas de palabras segun el operador que tenga delante.

            List<string> exc = new List<string>();  //signo de exclamacion (la palabra no esta en los txt)
            List<string> pico = new List<string>(); //piquito (la palabra tiene q estar en los txt)
            List<(string, int)> aster = new List<(string, int)>();      // asterisco (aumenta en tf)
            List<(string, string)> ene = new List<(string, string)>();  //tilde de la eñe (cercania)
            
            int contador = 0;
            
            for(int i = 0; i < query.Length; i++)
            { 
            
                switch (query[i])
                {
                    case '!': 
                    exc.Add(Dame_una_palabra( i, query)); 
                    break;
        
                    case '^':
                    pico.Add(Dame_una_palabra( i, query)); 
                    break;
        
                    case '*':
                    //cantidad de asterisco q tiene delante la palabra.
                    for (int k = i; query[k] == '*' && k < query.Length; k++)
                    {
                       contador++; 
                    } 
                    aster.Add((Dame_una_palabra( i+contador-1, query), contador)); 
                    i +=contador;
                    contador = 0;
                    break;
        
                    case '~': 
                    ene.Add((Dame_una_palabra( i, query), Dame_una_palabra2( i, query)));
                    break;       
        
                }
             
            } 
            return (exc, pico,aster,ene);
        }
     
    }  
          
}    