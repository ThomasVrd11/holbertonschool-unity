using UnityEngine;

public class ButtonsLinks : MonoBehaviour
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="link">0 Linkedin, 1 Github, 2 Portefolio, 3 CV, 4 mail</param>
    public void LinkToPage(int link)
    {
        Debug.Log("Link to page: " + link);
        switch (link)
        {
            case 0:
                Application.OpenURL("https://www.linkedin.com/in/taillepierrenicolas/");
                break;
            case 1:
                Application.OpenURL("https://github.com/TaillepierreN");
                break;
            case 2:
                Application.OpenURL("https://nicolastaillepierre.onfabrik.com");
                break;
            case 3:
                Application.OpenURL("https://drive.google.com/file/d/1qibO304axHAIA7B99OvdjeUEiw3a6g03/view?usp=sharing");
                break;
            case 4:
                Application.OpenURL("mailto:Taillepierren@gmail.com");
                break;
            default:
                break;
        }
    }
}
