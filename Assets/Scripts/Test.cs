using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Test : MonoBehaviour
{
    [SerializeField] public DiceCheck _intelligenceCheck;
    [SerializeField] private Dice _dicePrafab;
    [Space]
    [SerializeField] private string _title;
    [SerializeField] private string _substitle;
    [SerializeField] private int _difficulty;
    [Space]
    [SerializeField] private bool _useSerializableModificators;
    [SerializeField] private List<DiceModificatorData> _modificators;
    [SerializeField] private List<SerializableDiceModificatorData> _serializableModificators;

    private void Start()
    {
        _intelligenceCheck.Init(_title, _substitle, _difficulty, _dicePrafab,
            _useSerializableModificators ? _serializableModificators : _modificators);
        _intelligenceCheck.Show();

        Debug.Log("Press R to reload scene. Press S to reset dice check");
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            ReloadScene();
        }
        
        else if (Input.GetKeyDown(KeyCode.S))
        {
            ResetDiceCheck();
        }
    }

    public void Exit()
    {
        Application.Quit();
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(0);
    }

    public void ResetDiceCheck()
    {
        _difficulty = Random.Range(4, 20);
        _intelligenceCheck.Init(_title, _substitle, _difficulty, _dicePrafab,
        _useSerializableModificators ? _serializableModificators : _modificators);
        _intelligenceCheck.Show();
    }
}
