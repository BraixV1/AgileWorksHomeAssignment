'use client'
import React from 'react';
import axios from 'axios';
import {useRouter} from "next/navigation";
import {IFormInterface} from "@/components/IFormInterface";
import ICreateEditForm from "@/components/ICreateEditForm"


export default function Create() {
    const history = useRouter();
    const onSubmit = async (data: IFormInterface) => {
        try {
            await axios.post('http://localhost:5035/api/Tasks/AddTask', { // Updated endpoint URL
                description: data.description,
                createdAtDt: new Date(),
                hasToBeDoneAtDt: data.hasToBeDoneAtDt
            });
            history.push('/')
        } catch (error) {
            console.error('Error:', error);
        }
    };

    return (
        <div className="container">
            <main role="main" className="pb-3">
                <h1>Create</h1>

                <h4>ToDoTask</h4>
                <hr/>
                <div className="row">
                    <div className="col-md-4">
                        <ICreateEditForm defaultValues={{
                            description: '',
                            createdAtDt: new Date(),
                            hasToBeDoneAtDt: new Date()
                        }} onSubmit={onSubmit}/>
                    </div>

                </div>
            </main>
        </div>
    );
}
