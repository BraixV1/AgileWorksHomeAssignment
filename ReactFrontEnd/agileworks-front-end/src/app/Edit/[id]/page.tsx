'use client'
import React, {useEffect, useState} from 'react';
import { useForm, SubmitHandler} from 'react-hook-form';
import axios, {valueOf} from "axios";
import {useRouter} from "next/navigation";
import Link from "next/link";
import {useParams} from "react-router";
import * as sea from "node:sea";
import {useTaskLoader} from "@/components/TaskLoader";
import {IFormInterface} from "@/components/IFormInterface";


type IformInput = {
    description: string,
    createdAtDt: Date,
    hasToBeDoneAtDt: Date,
    completedAtDt: Date,
    id: string
}
export default function Edit({searchParams}: {searchParams?: IFormInterface}) {
    
    const {register, handleSubmit} = useForm<IformInput>();
    const router = useRouter();
    if (searchParams === undefined) {
        return null;
    }
    useTaskLoader(searchParams.id, searchParams);
    
    const onSubmit = async (data: IformInput) => {
        try {
            await axios.patch('http://localhost:5035/api/Tasks/UpdateTask/'+ data.id, { // Updated endpoint URL
                description: data.description,
                createdAtDt: new Date(),
                hasToBeDoneAtDt: data.hasToBeDoneAtDt,
                id: data.id
            });
            router.push('/')
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
                      <form onSubmit={handleSubmit(onSubmit)}>
                          <div className="form-group">
                              <label htmlFor="description">Description</label>
                              <input {...register('description')} value={searchParams.description} className="form-control" type="text"
                                     id="description"/>
                              <span className="text-danger"></span>
                          </div>

                          <div className="form-group">
                              <label htmlFor="hasToBeDoneAtDt">HasToBeDoneAtDt</label>
                              <input {...register('hasToBeDoneAtDt')} className="form-control" type="datetime-local"
                                     id="hasToBeDoneAtDt"/>
                              <span className="text-danger"></span>
                          </div>

                          <div className="form-group">
                              <input type="submit" value="Create" className="btn btn-primary"/>
                          </div>
                          <input {...register('id')} type="hidden" data-val="true" data-val-required="The Id field is required." id="Id"
                                 name="Id" value={searchParams.id}/>
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

