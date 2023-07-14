namespace MoogleEngine
{
    // frecuencia de las palabras en el texto y y cantidad de documentos en los q se encuentra
    public class Tf_Idf 
    {
        //resultado del calculo del TF e IDF. Esto me calcula el valor de las palabras segun la catidad de vaces que se repitan en los textos y en cuantis textos se encuentra.
        public static (float[] , float[]) Resultado(List<Dictionary<string,int>> textos, List<(string, float)> querys, List<(string,int)> aster, List<(string, string)> ene, bool[] b, List<List<string>> tex)
        {
            float[,] Tf = new float[querys.Count, textos.Count]; //va guardando los valores del tf
            float[] IDF = new float[querys.Count]; //va guardando los valores del idf
            float[] Tf_IDF = new float[textos.Count]; //guarda los valores de la multiplicacion de tf e idf que seria el valor de la palabra de la query en el texto
    
            for(int i = 0; i < querys.Count; i++) 
            {
                int frecuencia_idf = 0;
                for(int j = 0; j < textos.Count; j++)
                {
                    if(!b[j])
                    {
                        //Calculo del Tf
                        int frecuencia_tf = 0;
                        if(textos[j].ContainsKey(querys[i].Item1))
                            frecuencia_tf = textos[j][querys[i].Item1]; 
                        
        
                        frecuencia_tf *= (1 + Operadores.Multiplicar(aster, querys[i].Item1));  // aumenta el tf segun los asteriscos q tenga la palabra
                        Tf[i, j] = frecuencia_tf > 0? 1 + (float)Math.Log10(frecuencia_tf): 0;  // log para no trabajar con numeros grandes en el tf.
        
                        frecuencia_idf = frecuencia_tf > 0? frecuencia_idf + 1 : frecuencia_idf;
                    }
                   
                }

                //Calculo del IDF.
                if(frecuencia_idf != 0)
                IDF[i] = (float)Math.Log10(textos.Count/frecuencia_idf);
            }  

            //distancia menor entre palabras del texto(sombrero de la ene)
            int[,] menor = Operadores.Menor_distancia(ene,tex);
            int suma_ene = 0;
            
            //Multiplicacion del Tf_IDF para el valor del texto segun este.
            for(int i = 0; i < textos.Count; i++)
            { 
                for(int j = 0; j < querys.Count; j++)
                {
                    Tf_IDF[i] += Tf[j, i] * IDF[j] * querys[j].Item2;
                   
                }
                for (int k = 0; k < ene.Count; k++)
                {
                    suma_ene += 1000/(menor[k, i] +1); 
                }
                Tf_IDF[i] += suma_ene;
                suma_ene = 0;
            }
            return (Tf_IDF, IDF) ;
            
        } 
    
        //Metodo de ordenacion de mayor a menor de los titulos y textos segun el TF e IDF.
        public static void Ordenar(List<string>titulo, List<string>textos, float[] Tf_IDF)
        {
            float temp;  //variables temporales para el swap.
            string txt;  //variables temporales para el swap.
            string tit;  //variables temporales para el swap.
    
            for(int i = 0; i < Tf_IDF.Length; i++)
            {
                for(int j = i+1; j < Tf_IDF.Length; j++)
                {
                 
                  if(Tf_IDF[i] < Tf_IDF[j])
                  {
                    temp = Tf_IDF[j]; 
                    txt = textos[j];
                    tit = titulo[j]; 
    
                    Tf_IDF[j] = Tf_IDF[i];  //cambiar de lugar el mayor con el menor 
                    textos[j] = textos[i]; //cambiar de lugar el mayor con el menor 
                    titulo[j] = titulo[i]; //cambiar de lugar el mayor con el menor 
    
                    Tf_IDF[i] = temp; 
                    textos[i] = txt;
                    titulo[i] = tit;
                  }  
    
                }
                
            }
    
        }

        //Metodo de ordenacion de mayor a menor de las palabras de la query segun el TF e IDF.
        public static void OrdenarPalabrasQuery(List<(string, float)> query, float[] IDF)
        {
            float temp;    //variables temporales para el swap.
            string querys; //variables temporales para el swap.
            float valor;
            
            for(int i = 0; i < IDF.Length; i++)
            {
                for(int j = i+1; j < IDF.Length; j++)
                {
                 
                    if(IDF[i] < IDF[j])
                    {
                        temp = IDF[j]; 
                        querys = query[j].Item1;
                        valor = query[j].Item2;

                        IDF[j] = IDF[i];
                        query[j] = query[i];
    
                        IDF[i] = temp;
                        query[i] = (querys, valor);
                      
                    }  
    
                }
                
            }

        }
    
    }
}
