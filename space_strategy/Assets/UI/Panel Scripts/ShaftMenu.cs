using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ShaftMenu : MonoBehaviour
{
    [SerializeField] private Slider _HPslider;
    [SerializeField] private Slider _SPslider;
    [SerializeField] private Slider _unitSlider;

    private MineShaft _myShaft = null;

    public void ReloadPanel(MineShaft shaft)
    {
        _myShaft = shaft;
        
        ReloadSliders();
        ReloadUnitSlider();
    }

    private void ReloadSliders()
    {
        _HPslider.maxValue = 100; // 100 TODO
        _HPslider.minValue = 0;
        _HPslider.value = _myShaft.HealthPoints;

        _SPslider.maxValue = 100; // 100 TODO
        _SPslider.minValue = 0;
        _SPslider.value = _myShaft.ShieldPoints;
    }

    private void ReloadUnitSlider()
    {
        Debug.Log("Reload Unit Slider!");

        _unitSlider.onValueChanged.RemoveAllListeners();
        _unitSlider.maxValue = _myShaft.capacity;
        _unitSlider.value = _myShaft.test; // unitsWorkers.Count
        _unitSlider.onValueChanged.AddListener( delegate{AddWorker();} );
    }

    private void AddWorker()
    {
        if (_unitSlider.value > _myShaft.test) // unitsWorkers.Count
        {
            Debug.Log("Add Worker");
            _myShaft.test++;
        }
        if (_unitSlider.value < _myShaft.test) // unitsWorkers.Count
        {
            Debug.Log("Remove worker");
            _myShaft.test--;
        }
        ReloadUnitSlider();
    }
}
