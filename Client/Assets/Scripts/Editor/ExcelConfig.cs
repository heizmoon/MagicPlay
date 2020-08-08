using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorTool {
 
    public class ExcelConfig {
        /// <summary>
        /// 存放excel表文件夹的的路径，本例xecel表放在了"Assets/Excels/"当中
        /// </summary>
        public static readonly string excelsFolderPath = Application.dataPath + "/Data/";
 
        /// <summary>
        /// 存放Excel转化CS文件的文件夹路径
        /// </summary>
        public static readonly string assetPath = "Assets/Resources/DataAssets/";
    }
}
