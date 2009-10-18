#pragma once

using namespace System;
using namespace System::ComponentModel;
using namespace System::Collections;
using namespace System::Diagnostics;

using namespace OpenTK;

#include "coldet.h"

namespace coldet
{
	public ref class ColdetModel
	{
	public:
		void AddTriangle( float V1[], float V2[], float V3[] );
		void AddTriangle( float v1x, float v1y, float v1z,float v2x, float v2y, float v2z,float v3x, float v3y, float v3z );
		void AddTriangle( Vector3 ^v1, Vector3 ^v2, Vector3 ^v3 );

		void FinalizeMesh(void);
		
		bool RayCollision(Vector3 ^point,Vector3 ^vector, bool closest, float segmin, float segmax);
	
		Vector3^ GetCollisionPoint ();
	internal:
		ColdetModel();
		~ColdetModel();
		CollisionModel3D *model;

	};

	public ref class Coldet 
	{
	public:
		Coldet(void);
		~Coldet();

		static ColdetModel^ NewCollisionModel();
	};

	
}
