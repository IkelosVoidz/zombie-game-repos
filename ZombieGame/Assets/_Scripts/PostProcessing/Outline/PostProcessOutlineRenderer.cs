﻿using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class PostProcessOutlineRenderer : PostProcessEffectRenderer<PostProcessOutline>
{
    public override void Render(PostProcessRenderContext context)
    {
        PropertySheet sheet = context.propertySheets.Get(Shader.Find("Outline"));
        sheet.properties.SetFloat("_Tickness", settings.thickness);
        sheet.properties.SetFloat("_MinDepth", settings.depthMin);
        sheet.properties.SetFloat("_MaxDepth", settings.depthMax);



        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}