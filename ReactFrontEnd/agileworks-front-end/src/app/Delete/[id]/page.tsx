'use client';
import axios from "axios";
import Link from "next/link";
import React, {useEffect, useState } from "react";
import {useForm} from "react-hook-form";
import {useRouter} from "next/navigation";
import * as sea from "node:sea";
import {useTaskLoader} from "@/components/TaskLoader";
import {IFormInterface} from "@/components/IFormInterface";


export default function Delete({searchParams}: {searchParams?: IFormInterface}) {
    const {register, handleSubmit,} = useForm<IFormInterface>();
    const router = useRouter();
    
    if (searchParams === undefined) {
        return null;
    } 
    
    useTaskLoader(searchParams.id, searchParams);
    
    
    const onSubmit = async (data: IFormInterface) => {
        try {
            await axios.delete('http://localhost:5035/api/Tasks/DeleteTask/' + data.id, {id: data.id});
            router.push('/')
        } catch (error) {
            console.log('Error:', error);
        }
    }
    
    return (
        <div className="container">
            <main role="main" className="pb-3">


                <h1>Delete</h1>

                <h3>Are you sure you want to delete this?</h3>
                <div>
                    <h4>Task ID {searchParams.id}</h4>
                    <hr/>
                    <dl className="row">
                        <dt className="col-sm-2">
                            Description
                        </dt>
                        <dd className="col-sm-10">
                            {searchParams.description}
                        </dd>
                        <dt className="col-sm-2">
                            Created At
                        </dt>
                        <dd className="col-sm-10">
                            {searchParams.createdAtDt}
                        </dd>
                        <dt className="col-sm-2">
                            Has To be Done
                        </dt>
                        <dd className="col-sm-10">
                            {searchParams.hasToBeDoneAtDt}
                        </dd>
                    </dl>

                    <form onSubmit={handleSubmit(onSubmit)}>
                        <input {...register('id')} type="hidden" data-val="true" data-val-required="The Id field is required." id="Id"
                               name="Id" value={searchParams.id}/>
                        <input type="submit" value="Delete" className="btn btn-danger"/> |
                        <Link href={'../'}>Back to list</Link>
                   </form>
                    
                </div>

            </main>
        </div>
    );
}