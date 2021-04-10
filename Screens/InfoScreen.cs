using Rewired;
using UnityEngine;
using UnityEngine.UI;

public class InfoScreen : MonoBehaviour {
    private ScreenFader _fadeScr;
    private int _sceneNumb = 2;

    public Button Btn;
    public Text Cont;

    public int Index = 1;
    public Text Info1;
    public Text Info2;
    public Text Info3;
    public Text Info4;
    public Text Info5;
    public Text Info6;
    public Text Info7;
    public Text Info8;
    public Text Info9;

    private string _horizontalControls1;
    private string _verticalControls1;
    private string _upDownControls1;

    private void Start()
    {
        GetControls();
    }

    private void Update () {
        _fadeScr = GameObject.FindObjectOfType<ScreenFader>();
        DoTheDance();
    }

    public void DoTheDance()
    {
        if (Index == 1)
        {
            Info1.enabled = true;
            ShowContinue();
        }

        if (Index == 2)
        {
            Info2.enabled = true;
            ShowContinue();
        }

        if (Index == 3)
        {
            Info3.enabled = true;
            ShowContinue();
        }

        if (Index == 4)
        {
            Info4.enabled = true;
            ShowContinue();
        }

        if (Index == 5)
        {
            Info5.enabled = true;
            ShowContinue();
        }

        if (Index == 6)
        {
            Info6.enabled = true;
            ShowContinue();
        }

        if (Index == 7)
        {
            Info7.enabled = true;
            ShowContinue();
        }

        if (Index == 8)
        {
            Info8.enabled = true;
            ShowContinue();
        }

        if (Index == 9)
        {
            Info9.enabled = true;
            ShowContinue();
        }
    }

    private void ShowContinue()
    {
        Cont.enabled = true;
        Btn.GetComponent<Button>().enabled = true;
    }

    public void Continue () {

        if (Index == 1)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info1.enabled = false;
        }

        if (Index == 2)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info2.enabled = false;
        }

        if (Index == 3)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info3.enabled = false;
        }

        if (Index == 4)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info4.enabled = false;
        }

        if (Index == 5)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info5.enabled = false;
        }

        if (Index == 6)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info6.enabled = false;
        }

        if (Index == 7)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info7.enabled = false;
        }

        if (Index == 8)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info8.enabled = false;
        }

        if (Index == 9)
        {
            Cont.enabled = false;
            Btn.GetComponent<Button>().enabled = false;
            Info9.enabled = false;
        }

        Index += 1;
        Btn.GetComponent<Button>().enabled = false;

        if (Index == 10) _fadeScr.EndScene(_sceneNumb);
    }

    // very convoluted way to get the control scheme (and display it), but it works
    private void GetControls()
    {
        Player player = ReInput.players.GetPlayer(0);

        _horizontalControls1 = "To move horizontally use ";
        // Loop over all maps with the Action "Move Horizontal"
        int i = 0;
        foreach (var aem in player.controllers.maps.ButtonMapsWithAction("Move Horizontal", true))
        {
            Debug.Log(aem.elementIdentifierName);
            if (i != 1)
            {
                _horizontalControls1 += aem.elementIdentifierName + " and ";
                i += 1;
            }
            else
                _horizontalControls1 += aem.elementIdentifierName;
        }
        Info2.text = _horizontalControls1;

        Info2.text += "\n";
        _verticalControls1 = "To move vertically use ";
        // Loop over all maps with the Action "Move Vertical"
        int ii = 0;
        foreach (var aem in player.controllers.maps.ButtonMapsWithAction("Move Vertical", true))
        {
            Debug.Log(aem.elementIdentifierName);
            if (ii != 1)
            {
                _verticalControls1 += aem.elementIdentifierName + " and ";
                ii += 1;
            }
            else
                _verticalControls1 += aem.elementIdentifierName;
        }
        Info2.text += _verticalControls1;

        Info2.text += "\n";
        _upDownControls1 = "To move lower or higher use ";
        // Loop over all maps with the Action "Move UpDown"
        int iii = 0;
        foreach (var aem in player.controllers.maps.ButtonMapsWithAction("Move UpDown", true))
        {
            Debug.Log(aem.elementIdentifierName);
            if (iii != 1)
            {
                _upDownControls1 += aem.elementIdentifierName + " and ";
                iii += 1;
            }
            else
                _upDownControls1 += aem.elementIdentifierName;
        }
        Info2.text += _upDownControls1;
    }
}
