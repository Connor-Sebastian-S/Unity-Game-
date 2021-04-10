using UnityEngine;

public class EnvironmentColours : MonoBehaviour {

	public Color LColour;
	public Color RColour;

	public GameObject Player;

    private SettingsReader _settingsReader;

    private void Start()
    {
        _settingsReader = SettingsReader.GetInstance();
    }

    private void Update()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
        if (!Player) return;
        var s = _settingsReader.FoodWideSpreadY;
        var lerpParam = Mathf.InverseLerp(s, 0, Player.transform.position.y);
        RenderSettings.skybox.SetColor("_Color1", Color.Lerp(LColour, RColour, lerpParam));
    }
}
