'use client';
import axios from "axios";
import Link from "next/link";
import React, {useEffect, useState } from "react";
import {useForm} from "react-hook-form";
import {useRouter} from "next/navigation";
import {IFormInterface} from "@/components/IFormInterface";


export default function Delete({searchParams}: {searchParams?: IFormInterface}) {
    const {register, handleSubmit,} = useForm<IFormInterface>();
    const [loading, setLoading] = useState(true);
    const router = useRouter();
    
    if (searchParams === undefined) {
        return null;
    }
    if (searchParams.description === undefined) {

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
            await axios.delete('http://localhost:5035/api/Tasks/DeleteTask/' + data.id, {params: {id: data.id}});
            router.push('/')
        } catch (error) {
            console.log('Error:', error);
        }
    }

    if (loading) {
        return (
            <div className="spinner-border" role="status">
                <span className="sr-only">Loading...</span>
            </div>
        );
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
                        {searchParams.createdAtDt instanceof Date ? searchParams.createdAtDt.toISOString() : searchParams.createdAtDt}
                        </dd>
                        <dt className="col-sm-2">
                            Has To be Done
                        </dt>
                        <dd className="col-sm-10">
                        {searchParams.hasToBeDoneAtDt instanceof Date ? searchParams.hasToBeDoneAtDt.toISOString() : searchParams.hasToBeDoneAtDt}
                        </dd>
                    </dl>

                    <form onSubmit={handleSubmit(onSubmit)}>
                        <input {...register('id')} type="hidden" data-val="true" data-val-required="The Id field is required." id="Id"
                               name="Id" value={searchParams.id}/>
                        <input type="submit" value="Delete" className=""/> |
                        <Link href={'../'}>Back to list</Link>
                   </form>
                    
                </div>

            </main>
        </div>
    );
}