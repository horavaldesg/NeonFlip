using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelSelectorButton : MonoBehaviour
{
   [SerializeField] private TextMeshProUGUI levelNameText;
   private string levelName;
   
   public void SetText(string levelName)
   {
      var editedSceneName = levelName.Replace("Nf","");
      levelNameText.SetText(editedSceneName);
      SetLevelName(levelName);
   }

   private void SetLevelName(string levelName)
   {
      this.levelName = levelName;
   }

   public void ChangeLevel()
   {
      SceneManager.LoadScene(levelName);
      Time.timeScale = 1;
   }
}
