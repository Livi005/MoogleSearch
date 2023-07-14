namespace MoogleEngine
{
    public static class Sugerencias
    {
        //calcular distancia de Levenshtein
        public static int Levenshtein(string a, string b)
        {
            int i = a.Length - 1;
            int f = b.Length - 1;
            int value = int.MaxValue;
            Menor(i, f, a, b, ref value, 0);
            return value;

        }

        //cantidad de cambios q hay que hacer para convertir una palabra en otra.
        private static void Menor(int i, int f, string a, string b, ref int distancia, int current)
        {
            if (current < distancia && current < a.Length / 2)
            {
                // si alguna de las palabras llego a su final las variacione son las letras q queda de la otra.
                if (i < 0) { distancia = Math.Min(current + f + 1, distancia); return; }
                if (f < 0) { distancia = Math.Min(current + i + 1, distancia); return; }

                //si las letras son iguales compara las siguientes.
                if (a[i] == b[f]) { Menor(i - 1, f - 1, a, b, ref distancia, current); return; }

                current++;
                Menor(i - 1, f, a, b, ref distancia, current);  // operacion de insertar una letra.
                Menor(i, f - 1, a, b, ref distancia, current); // operacion de eliminar una letra.
                Menor(i - 1, f - 1, a, b, ref distancia, current); //operacion de cambiar una letra por otra.
            }

        }

        //palabras semejantas en los textos a las palabras de la query.
        public static List<string> Palabras_semejantes(List<(string, float)> query, List<Dictionary<string, int>> textos)
        {

            List<string> semejante = new List<string>();
            string palabra = "";
            int cambios = int.MaxValue; // menor numero de cambios que se va actualizando mientras encuentres uno menor

            for (int i = 0; i < query.Count; i++)
            {
                for (int j = 0; j < textos.Count; j++)
                {
                    foreach (var key in textos[j].Keys)
                    {
                        if (query[i].Item1 != key) // si la palabra de laquery es igual a la de el texto no hacer levenshtain.
                        {
                            int c = Levenshtein(query[i].Item1, key); //Levenshtein para la menor cantidad de cambios

                            if (c < cambios)
                            {
                                cambios = c;
                                palabra = key; // palabra con menor cantidad de cambios.

                            }
                        }

                    }

                }
                semejante.Add(palabra); //poner en la lista las palabras con menores cambios(mas semejantes)****
                cambios = int.MaxValue;
                palabra = "";
            }

            return semejante;

        }

        //sugiro las palabras parecidas a las de la query por si hay alguna mal escrita.
        public static string Sugerir(List<(string, float)> query, float[] IDF, List<string> Palabras_semejantes, int n)
        {
            string sugerir = ""; //resultado de la sugerencia.

            for (int i = 0; i < query.Count; i++)
            {
                //busco las palabras semejantes a las que tengan mayor IDF en la query
                if (IDF[i] >= 0.30)
                {
                    //si no hay palabras semejantes cojo la misma de la query mas un espacio.
                    if (Palabras_semejantes[i] == "") sugerir += query[i].Item1 + " ";
                    else sugerir += Palabras_semejantes[i] + " ";
                }
                else
                {
                    sugerir += query[i].Item1 + " ";
                }

            }

            return sugerir;
        }

    }
}