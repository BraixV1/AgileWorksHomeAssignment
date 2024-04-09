'use client'
import React from 'react';
import axios from 'axios';
import { useForm } from 'react-hook-form';
import { useRouter} from "next/navigation";
import Link from "next/link";
import {IFormInterface} from "@/components/IFormInterface";



export default function Create() {
    const form = useForm<IFormInterface>();
    const { register, handleSubmit } = form;
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
                <hr />
                <div className="row">
        <div className="col-md-4">
            <form onSubmit={handleSubmit(onSubmit)}>

                <div className="form-group">
                    <label htmlFor="description">Description</label>
                    <input {...register('description')} className="form-control" type="text" id="description" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-group">
                    <label htmlFor="hasToBeDoneAtDt">HasToBeDoneAtDt</label>
                    <input {...register('hasToBeDoneAtDt')} className="form-control" type="datetime-local" id="hasToBeDoneAtDt" />
                    <span className="text-danger"></span>
                </div>

                <div className="form-group">
                    <input type="submit" value="Create" className="btn btn-primary"/>
                </div>
            </form>
        </div>
                </div>

                <div>
                    <Link href={'../'}>Back to list</Link>
                </div>
            </main>
        </div>
    );
}
