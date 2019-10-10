using UnityEngine;
using UnityEditor;
using System.Linq;
using System;

[ExecuteInEditMode]
[CustomEditor(typeof(TasteCustomer))]//inside Typeof you put the name of the script you want to modify with this custom inspector
public class TasteCustomerEditor : Editor
{
    //These are the toolbar buttons you are going to see at the inspector
    private TasteType _toolbalTasteType;
    private SpotType _toolbalSpotType;
    private BeveragesType _toolbalBeveragesType;
    private MusicType _toolbalMusicType;
    private DialogType _toolbalDialogType;


    //these are to hold the current selection from the toolbars mentioned above
    private int _currentToolbarType;
    private int _currentToolbarSubType;

    //this works as a direct reference to the variables at your script with the Enums
    private TasteCustomer _myTasteCustomer;


    private void OnEnable()
    {
        _myTasteCustomer = base.target as TasteCustomer;
    }


    //This will "re-write" the way your script looks at the inspector tab
    public override void OnInspectorGUI()
    {
        ShowTasteCustomer();//this is were the magic happens, it shows the customized version of the enums
      //  EditorGUILayout.HelpBox("default base values showed above this message", MessageType.Info);
       // base.OnInspectorGUI();//this is the default way your script is showed at the inspector, leave it so you can see the changes happening in real-time
    }


    public void ShowTasteCustomer()
    {
        EditorGUILayout.LabelField("Taste Customer", EditorStyles.boldLabel);
        GUILayout.Space(5);
        GUILayout.Label("Type: ");
        _currentToolbarType = GUILayout.Toolbar(_currentToolbarType, Enum.GetNames(typeof(TasteType)));//this will display a toolbar with two options: (SUPPORT | OFFENSE)
        if (_currentToolbarType == 0) { TypeSpotSelected(); }
        if (_currentToolbarType == 1) { TypeLiquidSelected(); }
        if (_currentToolbarType == 2) { TypeMusicSelected(); }
        if (_currentToolbarType == 3) { TypeDialogSelected(); }
    }

    void TypeSpotSelected()//If you click the SUPPORT option, this new toolbar will be created right beneath the (SUPPORT | OFFENSE) toolbar
    {
        GUILayout.Label("Spot Type: ");
        _currentToolbarSubType = GUILayout.Toolbar(_currentToolbarSubType, Enum.GetNames(typeof(SpotType)));//this will display a toolbar with two options: (BLESS | HELP)
        _myTasteCustomer.SpotType = (SpotType)_currentToolbarSubType ;//IMPORTANT: This sets the Ability Type to Support at the original script
               
    }

    void TypeLiquidSelected()//If you click the SUPPORT option, this new toolbar will be created right beneath the (SUPPORT | OFFENSE) toolbar
    {
        GUILayout.Label("Liquid Type: ");
        _currentToolbarSubType = GUILayout.Toolbar(_currentToolbarSubType, Enum.GetNames(typeof(BeveragesType)));//this will display a toolbar with two options: (BLESS | HELP)
        _myTasteCustomer.BeveragesType = (BeveragesType)_currentToolbarSubType;//IMPORTANT: This sets the Ability Type to Support at the original script

    }
    void TypeMusicSelected()//If you click the SUPPORT option, this new toolbar will be created right beneath the (SUPPORT | OFFENSE) toolbar
    {
        GUILayout.Label("Music Type: ");
        _currentToolbarSubType = GUILayout.Toolbar(_currentToolbarSubType, Enum.GetNames(typeof(MusicType)));//this will display a toolbar with two options: (BLESS | HELP)
        _myTasteCustomer.MusicType = (MusicType)_currentToolbarSubType;//IMPORTANT: This sets the Ability Type to Support at the original script

    }
    void TypeDialogSelected()//If you click the SUPPORT option, this new toolbar will be created right beneath the (SUPPORT | OFFENSE) toolbar
    {
        GUILayout.Label("Sex Type: ");
        _currentToolbarSubType = GUILayout.Toolbar(_currentToolbarSubType, Enum.GetNames(typeof(DialogType)));//this will display a toolbar with two options: (BLESS | HELP)
        _myTasteCustomer.DialogType = (DialogType)_currentToolbarSubType;//IMPORTANT: This sets the Ability Type to Support at the original script

    }


}