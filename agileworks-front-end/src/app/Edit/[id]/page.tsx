'use client'
import React, {useEffect, useState} from 'react';
import axios from "axios";
import {useRouter} from "next/navigation";
import {IFormInterface} from "@/components/IFormInterface";
import ICreateEditForm from '@/components/ICreateEditForm';


export default function Edit({searchParams}: {searchParams?: IFormInterface}) {
    
    const router = useRouter();
    const [loading, setLoading] = useState(true);
    if (searchParams === undefined) {
        return null;
    }

    useEffect(() => {
        fetchData();
    }, []);


    const fetchData = async () => {
        try {
            const response = await axios.get(`http://localhost:5035/api/Tasks/GetTask/${searchParams.id}`);
            searchParams.description = response.data.description;
            searchParams.createdAtDt = response.data.createdAtDt;
            searchParams.hasToBeDoneAtDt = response.data.hasToBeDoneAtDt;
            searchParams.completedAtDt = response.data.hasToBeDoneAtDt;
            setLoading(false);
        } catch(error) {
            console.error('Error getting data for task' + error);
            setLoading(false);
        }
    };
    
    const onSubmit = async (data: IFormInterface) => {
        try {
            await axios.patch('http://localhost:5035/api/Tasks/UpdateTask/'+ data.id, { // Updated endpoint URL
                description: data.description,
                createdAtDt: searchParams.createdAtDt,
                hasToBeDoneAtDt: data.hasToBeDoneAtDt,
                id: data.id
            });
            router.push('/')
        } catch (error) {
            console.error('Error: ', error);
        }
    };

    if (loading) {
        return <div>Loading...</div>;
    }


    return (
      <div className="container">
          <main role="main" className="pb-3">
              <h1>Create</h1>

              <h4>ToDoTask</h4>
              <hr/>
              <div className="row">
                  <div className="col-md-4">
                    <ICreateEditForm defaultValues={searchParams} onSubmit={onSubmit}/>              
                  </div>
              </div>
          </main>
      </div>
  );
}

