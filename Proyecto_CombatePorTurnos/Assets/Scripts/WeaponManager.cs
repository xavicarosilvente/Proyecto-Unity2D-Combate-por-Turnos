using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    

    // cuantas armas podemos crear
    public List<WeaponProperties> Weapons; //<---- lista de las propiedades del arma.

    //para que me coja un arma aleatoria cualquier arma aleatoria
    public WeaponProperties GetRandomWeapon()
    {
        
        WeaponProperties TempWeapon = Weapons[Random.Range(0, Weapons.Count)]; // me cojera de entre todas las armas una entre el cero y todas las que hay!
        WeaponProperties ClonWeapon = new WeaponProperties(TempWeapon.Name, TempWeapon.ID, TempWeapon.Attack, TempWeapon.Defense, TempWeapon.Velocity, TempWeapon.Strength, TempWeapon.Image);


        return ClonWeapon; // cuando llamemos a la funcion GetRandomWeapon,el return me va a devolver un arma aleatorio!
    }

    public WeaponProperties GetWeaponByID(int IdWeapon)
    {
       
        WeaponProperties TempWeapon = Weapons[IdWeapon];
        WeaponProperties ClonWeapon = new WeaponProperties(TempWeapon.Name, TempWeapon.ID, TempWeapon.Attack, TempWeapon.Defense, TempWeapon.Velocity, TempWeapon.Strength, TempWeapon.Image);

        return ClonWeapon; // cuando llamemos a la funcion GetRandomWeapon,el return me va a devolver un arma aleatorio!
    }
    // una funcion para que me de armas aleatoriamente
    public List<WeaponProperties> GetListRandomWeapons()
    {
        int RandomNumWeapons = Random.Range(0, Weapons.Count);//<---- para que aleatoriamente me de de 0 armas al maximo aleatoriamente, si pongo de 0 hay posibilidad de que aparezcan sin armas.
        List<WeaponProperties> TempWeapons = new List<WeaponProperties>();//<----- para que no se repitan las armas , una lista que revise que no se repiten.
        for (int i = 0; i < RandomNumWeapons; i++) // un for con el numero de armas
        {
            // que me de un arma aleatoria
            WeaponProperties RandomWeapon = GetRandomWeapon();

            // aqui basicamente le digo que mientras RandomWeapon este dentro de la lista TempWeapons me coje otra arma distinta.
            while(CheckWeapon(TempWeapons,RandomWeapon)== true)
            {
                RandomWeapon = GetRandomWeapon();
            }
            TempWeapons.Add(RandomWeapon);
        }

        return TempWeapons;
    }
    // para comprobar que TempWeapons est añadido en la lista, aplicacion que determina que te dice si puedes o no añadir el arma
    private bool CheckWeapon(List<WeaponProperties> wp, WeaponProperties Weapon) // una lista de donde tengo que comprobar y una lista que nos dice si esta este arma
    {
        for (int i = 0; i < wp.Count; i++)
        {
            if(wp[i].ID == Weapon.ID)// si en algun momento  dentro de la lista que coincida con el Id me dira que si
            {
                return true;
            }
        }
        return false;// sino me devuelve que no
    }

   
	
}
