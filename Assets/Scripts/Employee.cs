using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Employee : MonoBehaviour
{

    //public float speed = 5.0f;
    public float notLookingDistance = 20.0f;
    public float halfLookingDistance = 10.0f;
    public float fullLookingDistance = 1.0f;

    private GameObject player;

    public GameObject Lara;
    public GameObject Lars;
    public GameObject Demi;
    public GameObject Dani;
    public GameObject Maya;
    public GameObject Regiena;

    private GameObject selectedEmployee;
    private List<int> chosenEmployees = new List<int>();

    public List<GameObject> employees = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectsWithTag("Player")[0];

        employees.Add(Lara);
        employees.Add(Lars);
        employees.Add(Demi);
        employees.Add(Dani);
        employees.Add(Maya);
        employees.Add(Regiena);

        chooseEmployee();

    }

    // Update is called once per frame
    void Update()
    {
        float distance = this.transform.position.x - player.transform.position.x;

        if (distance > halfLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = selectedEmployee.GetComponent<EmployeeSprites>().notLooking;
            this.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (distance > fullLookingDistance && distance < halfLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = selectedEmployee.GetComponent<EmployeeSprites>().halfLooking;
            this.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            this.gameObject.GetComponent<SpriteRenderer>().flipX = true;
        }
        else if (distance > 0 && distance < fullLookingDistance)
        {
            this.gameObject.GetComponent<SpriteRenderer>().sprite = selectedEmployee.GetComponent<EmployeeSprites>().fullLooking;
            this.gameObject.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            //employeeActive = false;
        }

        //transform.Translate(Vector3.left * speed * Time.deltaTime);

        //if(player.GetComponent<Slime>().CanMove())
        //{
        //    speed = 5.0f;
        //} else
        //{
        //   speed = 0;
        //}
    }

    void chooseEmployee()
    {
        if (PlayerPrefs.HasKey("EmployeeCount"))
        {
            if (PlayerPrefs.GetInt("EmployeeCount") < 5)
            {
                PlayerPrefs.SetInt("EmployeeCount", PlayerPrefs.GetInt("EmployeeCount") + 1);
            }
            else
            {
                PlayerPrefs.SetInt("EmployeeCount", 0);
            }
        }
        else
        {
            PlayerPrefs.SetInt("EmployeeCount", 0);
        } 

        print(PlayerPrefs.GetInt("EmployeeCount"));
        selectedEmployee = employees[PlayerPrefs.GetInt("EmployeeCount")];
    }
}
