#include "ManagedInterface.h"

using namespace coldet;

ColdetModel::ColdetModel()
{
	model = newCollisionModel3D();
}


ColdetModel::~ColdetModel()
{
	delete(model);
}

void ColdetModel::AddTriangle( float V1[], float V2[], float V3[] )
{
	model->addTriangle(V1,V2,V3);
}

void ColdetModel::AddTriangle( float v1x, float v1y, float v1z,float v2x, float v2y, float v2z,float v3x, float v3y, float v3z )
{
	model->addTriangle( v1x, v1y, v1z, v2x, v2y, v2z, v3x, v3y, v3z);
}

void ColdetModel::AddTriangle( Vector3 ^v1, Vector3 ^v2, Vector3 ^v3 )
{
	model->addTriangle(v1->X,v1->Y,v1->Z,v2->X,v2->Y,v2->Z,v1->X,v3->Y,v3->Z);
}

bool ColdetModel::RayCollision(Vector3 ^point,Vector3 ^vector, bool closest, float segmin, float segmax)
{
	float p[3],v[3];
	p[0] = point->X;
	p[1] = point->Y;
	p[2] = point->Z;
	v[0] = vector->X;
	v[1] = vector->Y;
	v[2] = vector->Z;

	return model->rayCollision(p,v,closest,segmin,segmax);
}


void ColdetModel::FinalizeMesh(void)
{
	model->finalize();
}

Coldet::Coldet(void){}
Coldet::~Coldet(){}

ColdetModel^ Coldet::NewCollisionModel()
{
	return gcnew ColdetModel();
}


