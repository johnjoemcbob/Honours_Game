using UnityEngine;
using UnityEngine.UI;
using System.Collections;

// Reset the menu UI when it is activated
// 
// Matthew Cormack
// 04/05/16 - 17:39

public class UIReset : MonoBehaviour
{
	void OnEnable()
	{
		transform.GetChild( 2 ).GetComponent<Text>().enabled = false;
		transform.GetChild( 3 ).GetComponent<Text>().enabled = false;
    }
}
