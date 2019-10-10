﻿
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(CustomerData))]
public class CustomerDataEditor : Editor
{

    private SerializedProperty m_TasteCustomerData;
    private ReorderableList m_ReorderableList;

    private void OnEnable()
    {
        m_TasteCustomerData = serializedObject.FindProperty("SpotsLiked");


        //Create an instance of our reorderable list.
        m_ReorderableList = new ReorderableList(serializedObject: serializedObject, elements: m_TasteCustomerData, draggable: true, displayHeader: true,
            displayAddButton: true, displayRemoveButton: true);

        //Set up the method callback to draw our list header
        m_ReorderableList.drawHeaderCallback = DrawHeaderCallback;

        //Set up the method callback to draw each element in our reorderable list
        m_ReorderableList.drawElementCallback = DrawElementCallback;

        //Set the height of each element.
        m_ReorderableList.elementHeightCallback += ElementHeightCallback;

        //Set up the method callback to define what should happen when we add a new object to our list.
        m_ReorderableList.onAddCallback += OnAddCallback;


    }

    public override void OnInspectorGUI()
    {
        CustomerData customerData = target as CustomerData; 
        base.DrawDefaultInspector();

        if(customerData.Template != null && !customerData.IsAlreadyTemplateLoaded)
        {
            customerData.TemplateLoad();
            customerData.IsAlreadyTemplateLoaded = true;
        }

       /* //Update serialized object's representation.
        serializedObject.Update();


        m_ReorderableList.DoLayoutList();
        //Draw the list property we found in the ScriptableObject
        // EditorGUILayout.PropertyField(property: m_TasteCustomerData, includeChildren: true);

        //Apply any changes made to the serializedObject and flush it's data stream.
        serializedObject.ApplyModifiedProperties();*/
    }

    /// <summary>
    /// Draws the header for the reorderable list
    /// </summary>
    /// <param name="rect"></param>
    private void DrawHeaderCallback(Rect rect)
    {
        EditorGUI.LabelField(rect, "Taste");
    }

    /// <summary>
    /// This methods decides how to draw each element in the list
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="index"></param>
    /// <param name="isactive"></param>
    /// <param name="isfocused"></param>
    private void DrawElementCallback(Rect rect, int index, bool isactive, bool isfocused)
    {
        //Get the element we want to draw from the list.
        SerializedProperty element = m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index);
        rect.y += 2;

        //We get the name property of our element so we can display this in our list.
        SerializedProperty elementName = element.FindPropertyRelative("name");
        string elementTitle = string.IsNullOrEmpty(elementName.stringValue)
            ? "Select Taste type Customer: "
            : $"Taste: {elementName.stringValue}";

        //Draw the list item as a property field, just like Unity does internally.
        EditorGUI.PropertyField(position:
            new Rect(rect.x += 10, rect.y, Screen.width * .8f, height: EditorGUIUtility.singleLineHeight), property:
            element, label: new GUIContent(elementTitle), includeChildren: true);
    }

    /// <summary>
    /// Calculates the height of a single element in the list.
    /// This is extremely useful when displaying list-items with nested data.
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private float ElementHeightCallback(int index)
    {
        //Gets the height of the element. This also accounts for properties that can be expanded, like structs.
        float propertyHeight =
            EditorGUI.GetPropertyHeight(m_ReorderableList.serializedProperty.GetArrayElementAtIndex(index), true);

        float spacing = EditorGUIUtility.singleLineHeight / 2;

        return propertyHeight + spacing;
    }

    /// <summary>
    /// Defines how a new list element should be created and added to our list.
    /// </summary>
    /// <param name="list"></param>
    private void OnAddCallback(ReorderableList list)
    {
        var index = list.serializedProperty.arraySize;
        list.serializedProperty.arraySize++;
        list.index = index;
        var element = list.serializedProperty.GetArrayElementAtIndex(index);
    }





}

