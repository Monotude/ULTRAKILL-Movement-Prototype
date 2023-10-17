using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.UI;

public class SetStaminaUI : MonoBehaviour
{
    private Slider staminaBar;
    private PlayerMovement stamina;

    void Start()
    {
        staminaBar = GetComponent<Slider>();
        stamina = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
    }

    void Update()
    {
        int dashRemaining = (int)stamina.CurrentStamina;
        if(staminaBar.value != dashRemaining)
        {
            staminaBar.value = dashRemaining;
        }
    }
}
