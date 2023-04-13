/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Inform�tica de la Universidad Complutense de Madrid (Espa�a).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisi�n: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using UnityEngine;

namespace es.ucm.fdi.iav.rts.gxx
{
    // Mapa de fuerza militar m�a y de mi enemigo
    public class RTSForceMap : RTSInfluenceMap
    {
        List<RTSWorldToMap.Info>[,] fmap;
        float myForce = 0.0f, enemyForce = 0.0f;

        public float MyForce() { return myForce; }
        public float EnemyForce() { return enemyForce; }

        // Se activa con la letra F
        private void Awake()
        {
            toggleVisible = KeyCode.F;
        }

        public override float[,] GetMap()
        {
            //usado para almacenar las influencias extendidas, para que estas no afecten otros c�lculos incompletos
            float[,] auxmap = new float[w, h];
            fmap = RTSGameManager.Instance.getForceMap();
            resetmap();

            // IMPLEMENTAR C�MO SE CONVIERTEN LOS DATOS BRUTOS EN UN MAPA DE INFLUENCIA (FUERZA), LLAMANDO A LOS M�TODOS AUXILIARES QUE QUER�IS

            return map;
        }


        // Dibuja la celda
        protected override void DrawCell(int i, int j) 
        {
            float intensity = map[i, j];

            Color color = Color.white;
            if (intensity > 0)
                color = Color.blue;
            else if (intensity < 0)
                color = Color.red;
            color.a = (float)(0.4 + 0.6 * Mathf.Min(10, Mathf.Abs(intensity) / 10));

            renderCubes[i, j].material.color = color;
        }

    }
}
