using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterManager : MonoBehaviour
{
    [System.Serializable]//<----para que se vean las clases, con esto ya se veran las listas de armas.


    public class CharacterProperties// caracteristicas del personaje.
    {
        public string Name;
        public int ID;
        public int Life;
        public Vector2 Attack;  // ataque del PERSONAJE, es vector 2 asi tenemos un minimo y un maximo de ataque.
        public Vector2 Defense;  //defensa del PERSONAJE, es vector 2 asi tenemos un minimo y un maximo de defensa.
        public int Velocity;  // velocidad del PERSONAJE
        public GameObject Char; // <--- esto es un prefab que es el que voy a instanciar, animaciones imagenes tendra todo!

        public List<WeaponProperties> Weapons;  //una lista de armas , porque cada personaje tendra sus armas


        //aqui igualamos el de arriba con el del parentesis, esto se tiene que hacer sino luego no puedo asignar, es necesario para generar diferentes personajes
        public CharacterProperties( string _Name, int _ID,int _Life, Vector2 _Attack, Vector2 _Defense, int _Velocity, GameObject _Char, List<WeaponProperties> _Weapons )
        {
            Name = _Name;
            ID = _ID;
            Life = _Life;
            Attack = _Attack;
            Defense = _Defense;                // con esto lo que hacemos es que los dos personajes al ser los mismos, seran diferenciados por sus caracteristicas, tendran valores aleatorios random.
            Velocity = _Velocity;
            Char = _Char;
            Weapons = _Weapons;
        }
    }

    public List<CharacterProperties> Characters; // lista de las propiedades de los personajes

   public  CharacterProperties GetRandomCharacter()//para que me de un Player "Character" aleatorio en la lista de Players en el script GameManager, un afuncion que me de un Char Random.
   {
        // para cojer un elemento random es a traves del parrafo de CharacterProperties.
        CharacterProperties TempChar = Characters[Random.Range(0, Characters.Count)];// de esta lista me cojera uno random entre todos los que hay, entre el cero y el final de la lista (Character.Count).
        CharacterProperties CloneChar = new CharacterProperties(TempChar.Name, TempChar.ID,TempChar.Life, TempChar.Attack, TempChar.Defense, TempChar.Velocity, TempChar.Char, TempChar.Weapons);// aqui he echo un Clon!

        return CloneChar; // cuando llamemos a la funcion GetRandomCharacter,el return me va a devolver un personaje aleatorio!
   }

    public CharacterProperties GetCharacterByID(int IdChar)//para que me de un Player "Character" aleatorio en la lista de Players en el script GameManager, un afuncion que me de un Char Random.
    {
        // para cojer un elemento random es a traves del parrafo de CharacterProperties.
        CharacterProperties TempChar = Characters[IdChar];// de esta lista me cojera uno random entre todos los que hay, entre el cero y el final de la lista (Character.Count).
        CharacterProperties CloneChar = new CharacterProperties(TempChar.Name, TempChar.ID, TempChar.Life, TempChar.Attack, TempChar.Defense, TempChar.Velocity, TempChar.Char, TempChar.Weapons);// aqui he echo un Clon!

        return CloneChar; // cuando llamemos a la funcion GetRandomCharacter,el return me va a devolver un personaje aleatorio!
    }



}
