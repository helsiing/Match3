using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEditor;
using VoodooMatch3.Models;

public class DataValidationTests
{
    public static List<LevelTemplate> GetLevelTemplatesList()
    {
        return AssetDatabase.FindAssets("t:LevelTemplate")
            .Select(guid => AssetDatabase.LoadAssetAtPath<LevelTemplate>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();
    }
    
    public static List<PieceTemplate> GetPieceTemplatesList()
    {
        return AssetDatabase.FindAssets("t:PieceTemplate")
            .Select(guid => AssetDatabase.LoadAssetAtPath<PieceTemplate>(AssetDatabase.GUIDToAssetPath(guid)))
            .ToList();
    }
    
    [Test]
    public void ValidateLevelTemplates([ValueSource(nameof(GetLevelTemplatesList))] LevelTemplate levelTemplate)
    {
        Assert.IsTrue(levelTemplate.ValidateConfig(), "Level template is not correctly configured");
    }
    
    [Test]
    public void ValidatePieceTemplates([ValueSource(nameof(GetPieceTemplatesList))] PieceTemplate pieceTemplate)
    {
        Assert.IsTrue(pieceTemplate.ValidateConfig(), "Piece template is not correctly configured");
    }
}
