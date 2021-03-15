/*
 * Copyright 2021 Google LLC
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.ARFoundation;

[RequireComponent(typeof(Light))]
public class LightEstimation : MonoBehaviour
{
    public ARCameraManager ARCameraManager;
    public Light Light;

    private void Start()
    {
        ARCameraManager.frameReceived += FrameReceived;

        Light = GetComponent<Light>();
    }

    private void FrameReceived(ARCameraFrameEventArgs args)
    {
        ARLightEstimationData lightEstimation = args.lightEstimation;

        if (lightEstimation.averageBrightness.HasValue)
        {
            Light.intensity = lightEstimation.averageBrightness.Value;
        }

        if (lightEstimation.averageColorTemperature.HasValue)
        {
            Light.colorTemperature = lightEstimation.averageColorTemperature.Value;
        }

        if (lightEstimation.colorCorrection.HasValue)
        {
            Light.color = lightEstimation.colorCorrection.Value;
        }

        if (lightEstimation.mainLightDirection.HasValue)
        {
            Light.transform.rotation = Quaternion.LookRotation(lightEstimation.mainLightDirection.Value);
        }

        if (lightEstimation.mainLightColor.HasValue)
        {
            Light.color = lightEstimation.mainLightColor.Value;
        }

        if (lightEstimation.mainLightIntensityLumens.HasValue)
        {
            Light.intensity = lightEstimation.averageMainLightBrightness.Value;
        }

        if (lightEstimation.ambientSphericalHarmonics.HasValue)
        {
            RenderSettings.ambientMode = AmbientMode.Skybox;
            RenderSettings.ambientProbe = lightEstimation.ambientSphericalHarmonics.Value;
        }
    }
}
