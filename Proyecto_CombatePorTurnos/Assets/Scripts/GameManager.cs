using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { INIT, WAIT, PLAYER1, PLAYER2, FINAL }//sistema de maquina de estados
    public GameState State; //<-------funcion para poder cambiar de estados
    [System.Serializable] //<---- para verlos desde fuera

    public class PlayersProperties
    {
        public CharacterManager.CharacterProperties Character;// accedo a CharacterProperties a traves del script characterManager.
        public Vector2 PositionInstance;// saber en que posicion del mundo tengo que intanciarlos

        [HideInInspector] public GameObject Pf_Player;//imprimo los personajes para que aparezcan en Unity , el [HideInInspector] es para que siga siendo publico pero no se ve fuera.
        public GameObject GridWeapon;// como cada player tiene un grid diferente lo ponemos aqui dentro de la clase


        [HideInInspector] public bool HasAttack;//esta boleana me dice si a atacado o no.el [HideInInspector] es para que siga siendo publico pero no se ve fuera.
        [HideInInspector] public bool GoAttack; // si estas atacando me dice si estas iendo o volviendoel [HideInInspector] es para que siga siendo publico pero no se ve fuera.

        [HideInInspector] public int CurrentLife;// la vida se pueda ir perdiendo con un Current Life
        public Image LifeBar; // asigno aqui la imagen de la barra de vida fuera
        public Animator Anim;
    }
    public List<PlayersProperties> Players;// una lista con los players, en este caso 2 el usario y el rival ( la lista se ve fuera en el script game manager)

    public GameObject BaseWeapons;// es el arma que voy a imprimir en el panel
    private int IndexWin; // para ver si ha ganado el player 1 o 2
    public GameObject AttackInfo; //para que se vea el daño en texto

    private CharacterManager.CharacterProperties SelectChar;
    private List<WeaponProperties> SelectWeapons = new List<WeaponProperties>();
    public GameObject DestruirBotonEspada, DestruirBotonAcha, DestruirBotonLanza, DestruirBotonKnife, DestruirBotonHammer;
    public bool Visible;
    public GameObject Parpadeo;
    public float Contador = 0;





    public GameObject GameScene, SelectCharScene;



    void Start()
    {



    }


    public void SelectCharButtons(int IdChar)
    {
        SelectChar = GetComponent<CharacterManager>().GetCharacterByID(IdChar);
    }
    public void SelectWeaponsButtons(int IdWeapon)
    {
        SelectWeapons.Add(GetComponent<WeaponManager>().GetWeaponByID(IdWeapon));
    }
    public void OneUseButtonEspada()
    {
        Destroy(DestruirBotonEspada);

    }
    public void OneUseButtonAcha()
    {
        Destroy(DestruirBotonAcha);

    }
    public void OneUseButtonKnife()
    {
        Destroy(DestruirBotonKnife);

    }
    public void OneUseButtonLanza()
    {
        Destroy(DestruirBotonLanza);
    }
    public void OneUseButtonHammer()
    {
        Destroy(DestruirBotonHammer);
    }
    public void ImagenParpadeo()// imagen del inicio que parpadea y dice sque selecciones arma y personaje.
    {
        Parpadeo.GetComponent<Image>().enabled = Visible;
        Contador = Contador + Time.deltaTime;
        if (Contador < 0.5f)
        {
            Visible = true;
        }
        if (Contador > 1f && Contador < 1.5f)
        {
            Visible = false;

        }
        if (Contador > 1.5f)
        {
            Contador = 0;
        }
    }


    public void SetCharacters()
    {
        SelectCharScene.SetActive(false);
        GameScene.SetActive(true);
        // rellenamos los stats del personaje de forma aleatoria, los personajes de la lista Players, pero se han de recorrer los players que hay y se hace mediante los For.
        for (int i = 0; i < Players.Count; i++)//<------ si pulsas el tabulador dos veces el for se escribe solo solo habria que cambiar la palabra Players.Count
        {
            if (i == 0)
            {
                Players[i].Character = SelectChar;
                Players[i].Character.Weapons = SelectWeapons;
            }
            if (i == 1)
            {
                //aqui dentro generamos los players "Characters"
                Players[i].Character = GetComponent<CharacterManager>().GetRandomCharacter();// la formula para cojer un personaje aleatorio, aqui llamo a la funcion GetRandomCharacter del script CharacterManager!.
                Players[i].Character.Weapons = GetComponent<WeaponManager>().GetListRandomWeapons();// aqui dentro esta el funcionamiento entero de las armas random que no se repitan etc..

                //for (int j = 0; j < Players[i].Character.Weapons.Count; j++)//aqui imprimo las armas
                // {
                //   GameObject NewWeapon = Instantiate(BaseWeapons, Players[i].GridWeapon.transform);//donde? en el player i en el gridweapon     <-----------este parrafo comentado lo hacemos abajo para poder imprimir en el orden correcto las armas del player 1 y 2
                //  NewWeapon.GetComponent<Image>().sprite = Players[i].Character.Weapons[j].Image; //aqui le asigno una imagen
                // }
            }

            PrintWeapons(Players[i].Character.Weapons, Players[i].GridWeapon, i);//<--- aqui en lugar de hacer el bucle le paso la funcion de PrintWeapons

            Players[i].Pf_Player = Instantiate(Players[i].Character.Char); //instanciar el Pf_Player en el mundo en el prefab
            Players[i].Pf_Player.transform.position = Players[i].PositionInstance;// coloco el objeto instanciado

            if (i == 1)//el personaje se gira
            {

                Players[i].Pf_Player.transform.localScale = new Vector3(-1, 1, 1);// el personaje 1 que es el de la izquierda lo roto para que mire hacia a derceha
            }

            Players[i].Anim = Players[i].Pf_Player.transform.GetChild(0).GetComponent<Animator>();//asigno la animacion

            Players[i].CurrentLife = Players[i].Character.Life; // con esto se muestra la vida actual cuando la vida llege a 0 lo mato

        }

        //Cuenta atras antes de empezar
        ChangeState(GameState.WAIT); // una vez alla echo la seleccion de personajes cambias
    }
    private void PrintWeapons(List<WeaponProperties> wp, GameObject Grid, int Index)
    {
        for (int i = 0; i < Grid.transform.childCount; i++)//para borra los hijos que me salen al lado del Grid sobrantes
        {
            Destroy(Grid.transform.GetChild(i).gameObject);
        }
        if (Index == 0)//imprimo las armas del player 1
        {
            for (int i = 0; i < wp.Count; i++)//aqui imprimo las armas
            {
                GameObject NewWeapon = Instantiate(BaseWeapons, Grid.transform);//donde? en el player i en el gridweapon
                NewWeapon.GetComponent<Image>().sprite = wp[i].Image; //aqui le asigno una imagen
            }
        }
        else//imprimo las armas del player 2
        {
            for (int i = wp.Count - 1; i >= 0; i--)
            {
                GameObject NewWeapon = Instantiate(BaseWeapons, Grid.transform);//donde? en el player i en el gridweapon
                NewWeapon.GetComponent<Image>().sprite = wp[i].Image; //aqui le asigno una imagen
            }
        }
    }


    void Update()
    {
        ImagenParpadeo();// imagen parpadeo seleccion de armas
        switch (State)
        {
            case GameState.INIT:

                break;
            case GameState.WAIT:
                Players[0].HasAttack = false;// los dos players no han atacado todavia
                Players[1].HasAttack = false;// los dos players no han atacado todavia

                //ahora establezco quien ataca primero, los dos player tienen una velocidad, suman la velocidad del player y la del arma.
                int IndexStart = CheckVelocityPlayers();
                print("Calculand datos de velocidad, empieza el Player: " + (IndexStart + 1));// cada vez que haga el WAIT me da esta info
                if (IndexStart == 0)
                {
                    Players[0].HasAttack = true;// en cuanto entre ya ha atacado
                    Players[0].GoAttack = true;//que va a atacar
                    ChangeState(GameState.PLAYER1);//Ataca el player 1

                }
                else
                {
                    Players[1].HasAttack = true;// en cuanto entre ya ha atacado
                    Players[1].GoAttack = true;//que va a atacar
                    ChangeState(GameState.PLAYER2);//Ataca el player 2

                }
                break;
            case GameState.PLAYER1:
                //va hacia el enemigo y luego vuelve
                if (Players[0].GoAttack == true)
                {
                    Players[0].Anim.SetBool("Move", true); // la animacion de correr sera tru cuando vaya a atacar!

                    // en lugar de ir justo a la posicion del otro player esta un cuadrado menos
                    Vector2 FinalPos = Players[1].PositionInstance;
                    FinalPos.x = FinalPos.x - 1;//<-------------posicion a la que tengo que ir


                    Players[0].Pf_Player.transform.position = Vector2.MoveTowards(Players[0].Pf_Player.transform.position, FinalPos, Players[0].Character.Velocity * Time.deltaTime);

                    if ((Vector2)Players[0].Pf_Player.transform.position == FinalPos)// si la posicion de este objeto es igual que la del final pos
                    {
                        int TempDamage = CheckAttack(0);//para poder utilizar el valo CheckAttack para hacer el texto con daño creo una variable que al cabo de un instantese destruye sola.

                        if (TempDamage == 0)
                        {
                            GameObject NewAttackInfo = Instantiate(AttackInfo, Players[1].Pf_Player.transform.position, Quaternion.identity);// aqui instancio el texto del daño o esquivado justo encima del contrincante
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Miss";// para modificar el valor que me muestra el texto, los dos getchild e spara acceder al hijo porque el texto esta en hijo del hijo.
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.green;// aqui le digo lo mismo que arriba solo que el color sea verde porque a parado el ataque.
                            Destroy(NewAttackInfo, 1);// destruye la instancia al cabo de un segundo
                        }
                        else
                        {
                            GameObject NewAttackInfo = Instantiate(AttackInfo, Players[1].Pf_Player.transform.position, Quaternion.identity);// aqui instancio el texto del daño o esquivado justo encima del contrincante
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "-" + TempDamage;// para modificar el valor que me muestra el texto, los dos getchild e spara acceder al hijo porque el texto esta en hijo del hijo.
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.red;// aqui le digo lo mismo que arriba solo que el color sea verde porque a parado el ataque.
                            Destroy(NewAttackInfo, 1);// destruye la instancia al cabo de un segundo
                        }

                        Players[1].CurrentLife -= TempDamage;// aqui justo le hago daño tanta como el check attack diga
                        Players[1].LifeBar.fillAmount = ((float)Players[1].CurrentLife / (float)Players[1].Character.Life);// la barra de vida sera equivalente a la vida actual del player, pongo delante de cada uno de los players el float para que no me baje toda la barra de vida.
                        Players[0].GoAttack = false;// el player1 ataca al player 2
                    }
                }
                else
                {
                    Players[0].Anim.SetBool("Move", true);// cuando se guira para volver vuelve a correr!

                    Players[0].Pf_Player.transform.localScale = new Vector3(-1, 1, 1);// al volver de atacar el player se da la vuelta
                    // en lugar de ir justo a la posicion del otro player esta un cuadrado menos
                    Vector2 FinalPos = Players[0].PositionInstance;



                    Players[0].Pf_Player.transform.position = Vector2.MoveTowards(Players[0].Pf_Player.transform.position, FinalPos, Players[0].Character.Velocity * Time.deltaTime);

                    if ((Vector2)Players[0].Pf_Player.transform.position == FinalPos)// si la posicion de este objeto es igual que la del final pos
                    {
                        Players[0].Anim.SetBool("Move", false); // aqui deja de atacar

                        Players[0].Pf_Player.transform.localScale = new Vector3(1, 1, 1);// al volver de atacar el player se da la vuelta
                        print("ya he terminado de atacar");
                        if (Players[1].CurrentLife > 0)
                        {
                            if (Players[1].HasAttack == false)//pregunto si el otro a atacado para pasar el turno
                            {
                                Players[1].HasAttack = true;// le digo que esta atacando
                                Players[1].GoAttack = true;
                                ChangeState(GameState.PLAYER2);
                            }
                            else
                            {
                                ChangeState(GameState.WAIT);
                            }
                            // mirar si el player 2 ya ha atacado
                            // si ya habia atacado, volver al waiting
                            // sino a atacado, pasar al estado de player 2
                        }
                        else
                        {
                            //el rival a perdido
                            IndexWin = 0;
                            ChangeState(GameState.FINAL);
                        }


                    }
                }
                break;
            case GameState.PLAYER2:
                if (Players[1].GoAttack == true)
                {
                    Players[1].Anim.SetBool("Move", true); // la animacion de correr sera tru cuando vaya a atacar!
                    Vector2 FinalPos = Players[0].PositionInstance;
                    FinalPos.x = FinalPos.x + 1;//<-------------posicion a la que tengo que ir

                    Players[1].Pf_Player.transform.position = Vector2.MoveTowards(Players[1].Pf_Player.transform.position, FinalPos, Players[1].Character.Velocity * Time.deltaTime);

                    if ((Vector2)Players[1].Pf_Player.transform.position == FinalPos)// si la posicion de este objeto es igual que la del final pos
                    {
                        int TempDamage = CheckAttack(1);//para poder utilizar el valo CheckAttack para hacer el texto con daño creo una variable que al cabo de un instantese destruye sola.

                        if (TempDamage == 0)
                        {
                            GameObject NewAttackInfo = Instantiate(AttackInfo, Players[0].Pf_Player.transform.position, Quaternion.identity);// aqui instancio el texto del daño o esquivado justo encima del contrincante
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Miss";// para modificar el valor que me muestra el texto, los dos getchild e spara acceder al hijo porque el texto esta en hijo del hijo.
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.green;// aqui le digo lo mismo que arriba solo que el color sea verde porque a parado el ataque.
                            Destroy(NewAttackInfo, 1);// destruye la instancia al cabo de un segundo
                        }
                        else
                        {
                            GameObject NewAttackInfo = Instantiate(AttackInfo, Players[0].Pf_Player.transform.position, Quaternion.identity);// aqui instancio el texto del daño o esquivado justo encima del contrincante
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "-" + TempDamage;// para modificar el valor que me muestra el texto, los dos getchild e spara acceder al hijo porque el texto esta en hijo del hijo.
                            NewAttackInfo.transform.GetChild(0).GetChild(0).GetComponent<Text>().color = Color.red;// aqui le digo lo mismo que arriba solo que el color sea verde porque a parado el ataque.
                            Destroy(NewAttackInfo, 1);// destruye la instancia al cabo de un segundo
                        }


                        Players[0].CurrentLife -= TempDamage;
                        Players[0].LifeBar.fillAmount = ((float)Players[0].CurrentLife / (float)Players[0].Character.Life);// la barra de vida sera equivalente a la vida actual del player, pongo delante de cada uno de los players el float para que no me baje toda la barra de vida.
                        Players[1].GoAttack = false;// el player0 go attack es falso
                    }
                }
                else
                {
                    Players[1].Anim.SetBool("Move", true); // la animacion de correr sera tru cuando vaya a atacar!
                    Players[1].Pf_Player.transform.localScale = new Vector3(1, 1, 1);// al volver de atacar el player se da la vuelta
                    Vector2 FinalPos = Players[1].PositionInstance;
                    Players[1].Pf_Player.transform.position = Vector2.MoveTowards(Players[1].Pf_Player.transform.position, FinalPos, Players[1].Character.Velocity * Time.deltaTime);

                    if ((Vector2)Players[1].Pf_Player.transform.position == FinalPos)// si la posicion de este objeto es igual que la del final pos
                    {
                        Players[1].Anim.SetBool("Move", false); // la animacion de correr sera tru cuando vaya a atacar!
                        Players[1].Pf_Player.transform.localScale = new Vector3(-1, 1, 1);// al volver de atacar el player se da la vuelta
                        print("ya he terminado de atacar");
                        if (Players[0].CurrentLife > 0)
                        {
                            if (Players[0].HasAttack == false)//pregunto si el otro a atacado para pasar el turno
                            {
                                Players[0].HasAttack = true;// le digo que esta atacando
                                Players[0].GoAttack = true;
                                ChangeState(GameState.PLAYER1);

                            }
                            else
                            {
                                ChangeState(GameState.WAIT);
                            }
                            // mirar si el player 1 ya ha atacado
                            // si ya habia atacado, volver al waiting
                            // sino a atacado, pasar al estado de player 1
                        }
                        else
                        {
                            IndexWin = 1;
                            ChangeState(GameState.FINAL);
                        }
                    }
                }
                break;
            case GameState.FINAL:
                print("Ha ganado el Player" + (IndexWin + 1) + "  con el nombre: " + Players[IndexWin].Character.Name);
                break;
        }
    }

    // esta funcion hace un checkatack
    private int CheckAttack(int Index)
    {
        // la suma de mi ataque mas la suma del ataque del arma
        int WeaponAttack = 0;
        if (Players[Index].Character.Weapons.Count > 0)
        {
            WeaponAttack = Random.Range((int)Players[Index].Character.Weapons[0].Attack.x, (int)Players[Index].Character.Weapons[0].Attack.y);

            //desgaste de arma
            Players[Index].Character.Weapons[0].Strength--;
            if (Players[Index].Character.Weapons[0].Strength <= 0)
            {
                Players[Index].Character.Weapons.RemoveAt(0);// cuando tengo el arma y golpe abra un desgaste cuando llege a 0 se la quito
                PrintWeapons(Players[Index].Character.Weapons, Players[Index].GridWeapon, Index);// al desgastar las armas y desaparecer hay que sacarla de la barra de armas fuera
            }


        }

        // fuerza final
        int FinalAttack = Random.Range((int)Players[Index].Character.Attack.x, (int)Players[Index].Character.Attack.y) + WeaponAttack;



        // cuanta defensa tiene el otro
        int RivalIndex = 0;
        if (Index == 0)
        {
            RivalIndex = 1;
        }

        // calculo defensa mi arma mas la suya y luego resta de mi ataque menos su defensa
        int WeaponDefense = 0;
        if (Players[RivalIndex].Character.Weapons.Count > 0)
        {
            WeaponDefense = Random.Range((int)Players[RivalIndex].Character.Weapons[0].Defense.x, (int)Players[RivalIndex].Character.Weapons[0].Defense.y);

            int RemoveWeapon = Random.Range(0, 100);// probabilidad random de quitarle el arma
            if (RemoveWeapon > Random.Range(70, 90))
            {
                Players[RivalIndex].Character.Weapons.RemoveAt(0);
                PrintWeapons(Players[RivalIndex].Character.Weapons, Players[RivalIndex].GridWeapon, RivalIndex);// con esta linea me imprimira cuando le quite el arma al enemigo se vera que desaparece tamb la imagen.
            }
        }
        // calcular el final defense
        int FinalDefense = Random.Range((int)Players[RivalIndex].Character.Defense.x, (int)Players[RivalIndex].Character.Defense.y) + WeaponDefense;

        int TotalAttackValue = FinalAttack - FinalDefense;
        if (TotalAttackValue < 0)
        {
            TotalAttackValue = 0;
        }



        return TotalAttackValue;

    }
    private int CheckVelocityPlayers()//<---- funcion que me calcula las velocidades asi sabemos quien empieza
    {
        //PLAYER 1
        int WeaponVelocityP1 = 0;//<------la velocidad sin arma
        if (Players[0].Character.Weapons.Count > 0)// el player 0 tiene armas?
        {
            WeaponVelocityP1 = Players[0].Character.Weapons[0].Velocity;// aqui le digo que siempre va atacar con la 0, cuando se le destruya la 0 sera otra arma
        }
        int TotalVelocityP1 = Players[0].Character.Velocity + WeaponVelocityP1; // aqui caculo la velocidad del player y el arma

        //PLAYER 2
        int WeaponVelocityP2 = 0;//<------la velocidad sin arma
        if (Players[1].Character.Weapons.Count > 0)// el player 0 tiene armas?
        {
            WeaponVelocityP2 = Players[1].Character.Weapons[0].Velocity;// aqui le digo que siempre va atacar con la 0, cuando se le destruya la 0 sera otra arma
        }
        int TotalVelocityP2 = Players[1].Character.Velocity + WeaponVelocityP2; // aqui caculo la velocidad del player y el arma



        if (TotalVelocityP1 >= TotalVelocityP2)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    private void ChangeState(GameState NewState)//esto se ha de crear siempre con una maquina de estados
    {
        State = NewState;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void ReturnMenu()
    {
        SceneManager.LoadScene(0);


    }
}
