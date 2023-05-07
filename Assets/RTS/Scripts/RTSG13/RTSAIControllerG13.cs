﻿/*    
   Copyright (C) 2020-2023 Federico Peinado
   http://www.federicopeinado.com

   Este fichero forma parte del material de la asignatura Inteligencia Artificial para Videojuegos.
   Esta asignatura se imparte en la Facultad de Informática de la Universidad Complutense de Madrid (España).

   Autores originales: Opsive (Behavior Designer Samples)
   Revisión: Federico Peinado 
   Contacto: email@federicopeinado.com
*/
using System.Collections.Generic;
using System;

namespace es.ucm.fdi.iav.rts.gxx
{
    using BehaviorDesigner.Runtime.Tactical.Tasks;
    using UnityEngine;

    public class RTSAIControllerG13 : RTSAIController
    {
        private int MyIndex { get; set; }
        private BaseFacility MyBaseFacility { get; set; }
        private BaseFacility OtherBaseFacility { get; set; }
        private ProcessingFacility MyProcessingFacility { get; set; }

        private RTSForceMap forceMap;
        private RTSValueMap valueMap;

        private float[,] fmap;
        private float[,] vmap;
        int w, h;
        float counter;

        public GameObject forceMapTile;
        public GameObject valueMapPillar;

        public struct CellInfo
        {
            Vector2Int coord;
            float total;
            public CellInfo(Vector2Int c, float t)
            {
                coord = c;
                total = t;
            }
            public static int greater(CellInfo a, CellInfo b)
            {
                if (a.total > b.total) return -1;
                return 1;
            }
            public Vector2Int getCoords() { return coord; }
            public float getTotal() { return total; }
        }

        // Número de paso de pensamiento 
        private int ThinkStepNumber { get; set; } = 0;

        // Última unidad creada
        private Unit LastUnit { get; set; }


        private List<BaseFacility> facilities;
        private List<ExtractionUnit> unitsExtractList;
        private List<ExplorationUnit> unitsExploreList;
        private List<DestructionUnit> unitsDestroyerList;

        //enemigos
        
        private List<ExtractionUnit> enemyUnitsExtractList;
        private List<ExplorationUnit> enemyUnitsExploreList;
        private List<DestructionUnit> enemyUnitsDestroyerList;

        // Despierta el controlado y configura toda estructura interna que sea necesaria
        // Se usan las teclas F (Fremen) o H (Harkonnen) según sea mi bando... y V para el valor de la casilla
        private void Start()
        {
            MyIndex = RTSGameManager.Instance.GetIndex(this);
            // Obtengo referencias a mis cosas
            MyBaseFacility = RTSGameManager.Instance.GetBaseFacilities(MyIndex)[0];
            List<int> OtherIndexes = RTSGameManager.Instance.GetIndexes();
            OtherIndexes.Remove(MyIndex); // Entiendo que no estoy modificando la lista original de índices...
            OtherBaseFacility = RTSGameManager.Instance.GetBaseFacilities(OtherIndexes[0])[0];
            MyProcessingFacility = RTSGameManager.Instance.GetProcessingFacilities(MyIndex)[0];

            forceMap = gameObject.AddComponent<RTSForceMap>();
            forceMap.RenderTile = forceMapTile;
            if(MyIndex == 0) forceMap.toggleVisible = KeyCode.F;
            else if(MyIndex == 1) forceMap.toggleVisible = KeyCode.H;
            valueMap = gameObject.AddComponent<RTSValueMap>();
            valueMap.RenderTile = valueMapPillar;
            valueMap.toggleVisible = KeyCode.V;

            w = RTSGameManager.Instance.getWidth();
            h = RTSGameManager.Instance.getHeight();
            counter = 0;


            facilities = RTSGameManager.Instance.GetBaseFacilities(MyIndex);
            unitsDestroyerList = RTSGameManager.Instance.GetDestructionUnits(MyIndex);
            unitsExploreList = RTSGameManager.Instance.GetExplorationUnits(MyIndex);
            unitsExtractList = RTSGameManager.Instance.GetExtractionUnits(MyIndex);

            enemyUnitsDestroyerList = RTSGameManager.Instance.GetDestructionUnits(OtherIndexes[0]);
            enemyUnitsExploreList = RTSGameManager.Instance.GetExplorationUnits(OtherIndexes[0]);
            enemyUnitsExtractList = RTSGameManager.Instance.GetExtractionUnits(OtherIndexes[0]);

        }

        // El método de pensar que sobreescribe e implementa el controlador, para percibir (hacer mapas de influencia, etc.) y luego ACTUAR.
        protected override void Think()
        {
            // IMPLEMENTAR AQUÍ VUESTRA LÓGICA, LLAMANDO A TODOS LOS MÉTODOS AUXILIARES QUE QUERÁIS
            
           //existen bases
            if (facilities.Count>0)
            {
                //decision de creacion de unidades
                if (unitsDestroyerList.Count<unitsExploreList.Count+unitsExtractList.Count)
                {
                    if (RTSGameManager.Instance.GetMoney(MyIndex) > RTSGameManager.Instance.DestructionUnitCost)
                        CreateDestructor();
                }
                else if (unitsExtractList.Count < unitsExploreList.Count)
                {
                    if (RTSGameManager.Instance.GetMoney(MyIndex) > RTSGameManager.Instance.ExtractionUnitCost)
                        CreateExtractor();
                }
                else
                    if (RTSGameManager.Instance.GetMoney(MyIndex) > RTSGameManager.Instance.ExplorationUnitCost)
                    CreateExplorator();
            }


            //Cuando exista cierto nº de tropas estas tomarán la decision de ir juntas a atacar
            //sino se quedan en la base para defender

            
            if (unitsDestroyerList.Count>5 && unitsExploreList.Count > 5)
            {
                for(int i = 0; i < 3; i++) { 
                   
                    RTSGameManager.Instance.MoveUnit(this, unitsDestroyerList[i], OtherBaseFacility.transform);
                   
                    RTSGameManager.Instance.MoveUnit(this, unitsExploreList[i], OtherBaseFacility.transform);
                }

            }
            //mientras no sea el maximo numero de unidades de exploracion o de destructores, los extractores sguen yendo a recoger
            if (unitsExploreList.Count < RTSGameManager.Instance.ExplorationUnitsMax|| unitsDestroyerList.Count < RTSGameManager.Instance.DestructionUnitsMax)
            {
                for(int i = 0; i < unitsExtractList.Count; i++) { 
                RTSGameManager.Instance.MoveUnit(this, unitsExtractList[i], MyProcessingFacility.transform);
                }
            }
        }

        // ..............
        // AQUÍ ALGUNOS MÉTODOS AUXILIARES DE EJEMPLO

        private void CreateExtractor()
        {
            if (RTSGameManager.Instance.GetMoney(MyIndex) >= RTSGameManager.Instance.ExtractionUnitCost)
            {
                List<BaseFacility> facilities = RTSGameManager.Instance.GetBaseFacilities(MyIndex);
                if (facilities.Count > 0)
                {
                    // Se pasa una instalación base cualquiera como parámetro (aleatoria, obviamente nuestra) 
                    Unit unit = RTSGameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], RTSGameManager.UnitType.EXTRACTION);

                    // Manda al extractor a un campo aleatorio
                    List<LimitedAccess> resourcesList = RTSScenarioManager.Instance.LimitedAccesses;
                    LimitedAccess resource = resourcesList[Random.Range(0, resourcesList.Count - 1)];

                    RTSGameManager.Instance.MoveUnit(this, unit, resource.transform);
                }
            }
        }

        private void CreateExplorator()
        {
            if (RTSGameManager.Instance.GetMoney(MyIndex) >= RTSGameManager.Instance.ExplorationUnitCost)
            {
                List<BaseFacility> facilities = RTSGameManager.Instance.GetBaseFacilities(MyIndex);
                if (facilities.Count > 0)
                    // Se pasa una instalación base cualquiera como parámetro (aleatoria, obviamente nuestra) 
                    RTSGameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], RTSGameManager.UnitType.EXPLORATION);
            }
        }

        private void CreateDestructor()
        {
            if (RTSGameManager.Instance.GetMoney(MyIndex) >= RTSGameManager.Instance.DestructionUnitCost)
            {
                List<BaseFacility> facilities = RTSGameManager.Instance.GetBaseFacilities(MyIndex);
                if (facilities.Count > 0)
                    // Se pasa una instalación base cualquiera como parámetro (aleatoria, obviamente nuestra) 
                    RTSGameManager.Instance.CreateUnit(this, facilities[Random.Range(0, facilities.Count - 1)], RTSGameManager.UnitType.DESTRUCTION);
            }
        }
    }
}
