using UnityEngine;

[System.Serializable]//<----para que se vean las clases, con esto ya se veran las listas de armas.
//creacion del arma
public class WeaponProperties // <----propiedades de las armas.
{
    public string Name;  // <--- nombre del arma
    public int ID;  // Id del arma.
    public Vector2 Attack;  // ataque del arma, es vector 2 asi tenemos un minimo y un maximo de ataque.
    public Vector2 Defense;  //defensa del arma, es vector 2 asi tenemos un minimo y un maximo de defensa.
    public int Velocity;  // velocidad del arma
    public int Strength;  // desgaste del arma

    public Sprite Image;  //imagen del arma


    //Funcion "constructor" de la clase. Esto es un concepto de Programación Orientada a Objetos (POO)
    public WeaponProperties(string _Name, int _ID, Vector2 _Attack,
        Vector2 _Defense, int _Velocity, int _Strength, Sprite _Image)//<----------genera armas para luego darselas a los personajes, aqui se pone en el parentesis todo lo de arriba pero con barra baja.
    {
        //aqui igualamos el de arriba con el del parentesis, esto se tiene que hacer sino luego no puedo asignar armas al personaje!.
        Name = _Name;
        ID = _ID;
        Attack = _Attack;
        Defense = _Defense;
        Velocity = _Velocity;
        Strength = _Strength;
        Image = _Image;

    }

    
}
